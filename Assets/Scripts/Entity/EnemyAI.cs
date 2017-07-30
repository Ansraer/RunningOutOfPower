using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

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

                EntityPlayer[] players = UnityEngine.Object.FindObjectsOfType<EntityPlayer>();
                foreach (EntityPlayer p in players)
                {
                    if (Vector3.Distance(this.transform.position, p.transform.position) <= this.maxDetectionDistance)
                    {

                        this.target = p.gameObject;
                        yield return null;
                        yield break;
                    }
                }

                
                GameObject hq = UnityEngine.Object.FindObjectOfType<BuildingHQ>().gameObject;
                if (hq != null)
                    this.target = hq;


            }

            //do something with target

            this.handleTarget();


            yield return null;
        }

        // EXIT THE IDLE STATE

    }


    IEnumerator WALK()
    {

        var seeker = GetComponent<Seeker>();
        // Start a new path request from the current position to a position 10 units forward.
        // When the path has been calculated, it will be returned to the function OnPathComplete unless it was canceled by another path request
        seeker.StartPath(transform.position, this.target.transform.position, OnPathComplete);

        currentWaypoint = 0;


        // EXECUTE IDLE STATE
        while (state == ENEMY_STATE.WALK)
        {
            //restart in case path hasn't been generated yet
            if (this.path == null) {
                yield return null;
                continue;
            }


            if (Time.time - lastRepath > repathRate && seeker.IsDone())
            {
                lastRepath = Time.time + UnityEngine.Random.Range(0f, repathRate) + repathRate;
                // Start a new path to the targetPosition, call the the OnPathComplete function
                // when the path has been calculated (which may take a few frames depending on the complexity)
                seeker.StartPath(transform.position, this.target.transform.position, OnPathComplete);
            }


            //make sure that the waypoint actually exists
            if (currentWaypoint > path.vectorPath.Count)
            {
                this.state = ENEMY_STATE.IDLE;
                this.path = null;
                rb2d.velocity = new Vector3(0,0,0);


                yield return null;
                yield break;
            }

            if (currentWaypoint == path.vectorPath.Count)
            {
                this.state = ENEMY_STATE.IDLE;
                this.path = null;
                rb2d.velocity = new Vector3(0,0,0);


                yield return null;
                yield break;
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


            //update target
            this.handleTarget();

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

            //don't move when idle
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

    //changes state depending on target, dissmises unrealistic target
    private void handleTarget()
    {
        if (this.target != null && target.GetComponent<Entity>().health > 0)
        {

            //attack the enemy if he is close enough
            if (Vector3.Distance(this.transform.position, this.target.transform.position) <= this.maxAttackDistance)
            {
                state = ENEMY_STATE.ATTACK;

            }
            else if (Vector3.Distance(this.transform.position, this.target.transform.position) > this.maxAttackDistance)
            {

                //only walk towards far away target if it is not a player
                if (this.target.GetComponent<EntityPlayer>() != null && Vector3.Distance(this.transform.position, this.target.transform.position) > this.maxChaseDistance)
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