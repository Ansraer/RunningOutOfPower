using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : EntityLiving
{


    public ENEMY_STATE state;


    //how close the enemy has to be to attack
    public float maxAttackDistance = 5;

    //how close something has to be to become the new primary target
    public float maxDetectionDistance = 6;

    //how far away something has to be for the enemy to move towards it
    public float minDetectionDistance = 1.2f;

    public float maxChaseDistance = 15;

    public int scoreValue = 10;

    public float attackRate = .3f;
    private float lastAttack=-9999999;
    public DamageResistance[] attackDamages;

    private GameObject target;

    private Rigidbody2D rb2d;

    private Path path;


    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 1;
    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;
    // How often to recalculate the path (in seconds)
    public float repathRate = 0.5f;
    private float lastRepath = -9999;

    #region UNITY METHODS

    public override void Awake()
    {
        base.Awake();
        state = ENEMY_STATE.IDLE;
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(EnemyFSM());
    }

    public override void Dead()
    {
        this.health = 0;

        GameManager.totalScore += this.scoreValue;
        Destroy(this.gameObject);
    }

    void OnDrawGizmos()
    {
        

        Gizmos.DrawSphere(
        this.transform.position + this.transform.rotation * (Vector2.up * 0.9f), 0.1f);
    }


    #endregion







    #region ENEMY COROUTINES


    //starts a new Coroutine once the old one ends
    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(state.ToString());
        }
    }


    IEnumerator IDLE()
    {

        // EXECUTE IDLE STATE
        while (state == ENEMY_STATE.IDLE)
        {
            //target, fallback is building hq

            //depending on type of target, distance and  either attack it or walk towards it.

            //don't move when idle
            this.rb2d.velocity = new Vector3();


            //aqcuire tagert, default to 
            if (this.target == null)
            {
                GameObject tempTarget = FindTarget();


                if (tempTarget == null)
                {

                    BuildingHQ hq = UnityEngine.Object.FindObjectOfType<BuildingHQ>();
                    //if there is really no other option, attack hq
                    if (hq != null)
                        this.target = hq.gameObject;

                } else
                {
                    target = tempTarget;
                }

            }

            //do something with target

            this.handleTarget();


            yield return null;
        }

        // EXIT THE IDLE STATE

    }


    IEnumerator WALK()
    {
        if (this.target == null)
        {

            this.state = ENEMY_STATE.ATTACK;
            this.path = null;
            rb2d.velocity = new Vector3(0, 0, 0);

            yield return null;
            yield break;
        }


            var seeker = GetComponent<Seeker>();
        // Start a new path request from the current position to a position 10 units forward.
        // When the path has been calculated, it will be returned to the function OnPathComplete unless it was canceled by another path request
        seeker.StartPath(transform.position, this.target.transform.position, OnPathComplete);

        currentWaypoint = 0;


        // EXECUTE IDLE STATE
        while (state == ENEMY_STATE.WALK)
        {
            //update target
            this.handleTarget();

            //restart in case path hasn't been generated yet
            if (this.path == null) {
                yield return null;
                continue;
            }


           


            // Direction to the next waypoint
            Vector3 dir1 = (path.vectorPath[currentWaypoint] - transform.position).normalized;

            float angle1 = Mathf.Atan2(dir1.y, dir1.x) * Mathf.Rad2Deg - 90;

            //CHECK IF SOMETHING IS BLOCKING THE WAY
            // do this shit less often
            Collider2D collider = Physics2D.OverlapCircle(this.transform.position + Quaternion.AngleAxis(angle1, Vector3.forward) * (Vector2.up * 0.9f), 0.1f);
            

            if (collider != null && collider.gameObject.GetComponent<Entity>() != null)
            {
                if(collider.gameObject == this.target)
                {
                    //THATS THE ENEMY!!!! KILL IT!
                    this.state = ENEMY_STATE.ATTACK;
                    this.path = null;
                    rb2d.velocity = new Vector3(0, 0, 0);

                    yield return null;
                    yield break;
                } else if (collider.gameObject.GetComponent<Building>() != null || collider.gameObject.GetComponent<EntityPlayer>() != null)
                {
                    //You are running into a building/player idiot. DESTROY IT!
                    this.target = collider.gameObject;
                    this.state = ENEMY_STATE.ATTACK;
                    this.path = null;
                    rb2d.velocity = new Vector3(0, 0, 0);

                    yield return null;
                    yield break;
                }
                /**
                else if (collider.gameObject.GetComponent<EntityForceField>() != null)
                {
                    //Damn it. there is a forcefield in front of us. Shit, shit shit.
                    //try to break through
                    this.target = collider.gameObject;
                    this.state = ENEMY_STATE.ATTACK;
                    this.path = null;

                    rb2d.velocity = new Vector3(0, 0, 0);

                    yield return null;
                    yield break;
                }
    */
                else if (collider.gameObject.GetComponent<EnemyAI>() != null)
                {
                    //You are running into a mate idiot. Let him pass

                    rb2d.velocity = new Vector3(0, 0, 0);

                    this.target = null;
                    handleTarget();

                    yield return null;
                    yield break;
                }


            }


            if (Time.time - lastRepath > repathRate && seeker.IsDone())
            {
                lastRepath = Time.time + UnityEngine.Random.Range(0f, repathRate) + repathRate;
                // Start a new path to the targetPosition, call the the OnPathComplete function
                // when the path has been calculated (which may take a few frames depending on the complexity)
                seeker.StartPath(transform.position, this.target.transform.position, OnPathComplete);
            }





            // Direction to the next waypoint
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            dir *= this.GetMovementSpeed();
            // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
            rb2d.velocity = dir;

            //make sure that Enemy looks forward
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // The commented line is equivalent to the one below, but the one that is used
            // is slightly faster since it does not have to calculate a square root
            //if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
            if ((transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance * nextWaypointDistance)
            {
                currentWaypoint++;
            }


            //make sure that the waypoint actually exists
            if (currentWaypoint >= path.vectorPath.Count)
            {
                this.state = ENEMY_STATE.IDLE;
                this.path = null;
                rb2d.velocity = new Vector3(0, 0, 0);
                currentWaypoint = 0;

                yield return null;
                yield break;
            }
            yield return null;

        }

        // EXIT THE WALKING STATE

    }


    IEnumerator ATTACK()
    {
        // EXECUTE ATTACK STATE
        while (state == ENEMY_STATE.ATTACK)
        {

            if(target == null || target.GetComponent<Entity>().health <= 0)
            {
                this.handleTarget();
                yield return null;
                yield break;
            }

            //don't move when attacking
            this.rb2d.velocity = new Vector3();

            //look at target
            Vector3 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (Time.time - lastAttack > attackRate)
            {
                lastAttack = Time.time + UnityEngine.Random.Range(0f, attackRate) + attackRate/2;
                

                if(target.GetComponent<Entity>() != null)
                {
                    foreach (DamageResistance a in this.attackDamages)
                    {
                        target.GetComponent<Entity>().TakeDamage(a.type, a.amount);
                    }
                }
            }

            //update target
            this.handleTarget();

            yield return null;
        }
        yield return null;
    }

    #endregion


    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            this.path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    private GameObject FindTarget()
    {

        EntityPlayer[] players = UnityEngine.Object.FindObjectsOfType<EntityPlayer>();
        foreach (EntityPlayer p in players)
        {
            if (Vector3.Distance(this.transform.position, p.transform.position) <= this.maxDetectionDistance)
            {
                //only target Player if he is not under the forcefield
                if (!GameManager.instance.forceFieldActive || Vector3.Distance(p.transform.position, new Vector3()) > GameManager.instance.forceFieldRadius)
                {

                    if(p.health>0)
                        return p.gameObject;
                    
                }
            }
        }

        //target HQ building if its not protected
        BuildingHQ hq = UnityEngine.Object.FindObjectOfType<BuildingHQ>();
        if (hq != null && !GameManager.instance.forceFieldActive)
        {
            if(hq.health>0)
                return hq.gameObject;
            

        }


        //target nearby building
        Building[] bs = UnityEngine.Object.FindObjectsOfType<Building>();

        foreach (Building b in bs)
        {
            if (b != null)
            {
                if (!GameManager.instance.forceFieldActive || Vector3.Distance(b.transform.position, new Vector3()) > GameManager.instance.forceFieldRadius)
                {


                    if (b.health > 0)
                    {
                        return b.gameObject;


                    }

                }

            }
        }

        return null;
    }

    //changes state depending on target, dissmises unrealistic target
    private void handleTarget()
    {
        if (this.target != null && target.GetComponent<Entity>().health > 0)
        {
            //check if the target is a forcefield or protected by one
            if (target.GetComponent<EntityForceField>() != null || (GameManager.instance.forceFieldActive && Vector3.Distance(target.transform.position, new Vector3()) < GameManager.instance.forceFieldRadius))
            {

                //target is under force field. Shit.
                //check if there is a better target somewhere.

                GameObject tempTarget = FindTarget();

                if (tempTarget != null)
                {


                    if (tempTarget != target)
                    {

                    this.target = tempTarget;
                    return;
                    }


                } else
                {
                    if (UnityEngine.Object.FindObjectOfType<EntityForceField>() != null)
                    {
                        this.target = UnityEngine.Object.FindObjectOfType<EntityForceField>().gameObject;
                    }
                }
            }


            float distance = 0;

            Collider2D targetCollider = target.GetComponent<Collider2D>();
            if (targetCollider != null)
            {
                distance = targetCollider.Distance(this.GetComponent<Collider2D>()).distance;
            } else {
                distance = Vector3.Distance(this.transform.position, this.target.transform.position);
            }
                


            //attack the enemy if he is close enough
            if (distance <= this.maxAttackDistance)
            {
                state = ENEMY_STATE.ATTACK;

            }
            else if (distance > this.maxAttackDistance)
            {
                //only walk towards far away target if it is not a player
                if (this.target.GetComponent<EntityPlayer>() != null && distance > this.maxChaseDistance)
                {
                    this.target = null;
                    this.path = null;
                    this.state = ENEMY_STATE.IDLE;
                }
                else
                {
                    this.state = ENEMY_STATE.WALK;
                }

            }
            else
            {
                this.state = ENEMY_STATE.WALK;
            }

        } else
        {
            this.target = null;
            this.path = null;
            this.state = ENEMY_STATE.IDLE;

        }
    }

}


public enum ENEMY_STATE
{
    IDLE,
    WALK,
    ATTACK
}