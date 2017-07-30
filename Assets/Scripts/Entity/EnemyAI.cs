using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{


    public ENEMY_STATE state;

    //HQ building
    public GameObject defaultTarget;


    //how close the enemy has to be to attack
    public float maxAttackDistance = 5;

    //how close something has to be to become the new primary target
    public float maxDetectionDistance = 6;

    //how far away something has to be for the enemy to move towards it
    public float minDetectionDistance = 1.2f;

    private GameObject target;

    private Rigidbody2D rb2d;

    private Path path;


    // The AI's speed in meters per second
    public float speed = 2;
    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 1;
    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;
    // How often to recalculate the path (in seconds)
    public float repathRate = 0.5f;
    private float lastRepath = -9999;

    #region UNITY METHODS

    public void Awake()
    {
        state = ENEMY_STATE.IDLE;
    }

    public void Start()
    {
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



            this.rb2d.velocity = new Vector3();
            EntityPlayer[] players = UnityEngine.Object.FindObjectsOfType<EntityPlayer>();



            foreach(EntityPlayer p in players)
            {
                if(Vector3.Distance(this.transform.position, p.transform.position) <= this.maxDetectionDistance)
                {
                    if (Vector3.Distance(this.transform.position, p.transform.position) >= this.minDetectionDistance) {
                        state = ENEMY_STATE.WALK;
                        this.target = p.gameObject;
                    } else
                    {
                        //attack
                    }

                    yield return null;
                    yield break;

                }
            }


            BuildingHQ hq = UnityEngine.Object.FindObjectOfType<BuildingHQ>();
            if (hq == null)
                Debug.Log("NULL");
            if (Vector3.Distance(this.transform.position, hq.transform.position) > this.maxAttackDistance)
            {
                //state = ENEMY_STATE.WALK;
                //this.target = hq.gameObject;
            }
            else
            {
                //attack building
            }



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
                continue;
            }

            if (currentWaypoint == path.vectorPath.Count)
            {
                this.state = ENEMY_STATE.IDLE;
                this.path = null;
                rb2d.velocity = new Vector3(0,0,0);


                Debug.Log("End Of Path Reached");
                yield return null;
                continue;
            }


            // Direction to the next waypoint
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            dir *= speed;
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


            yield return null;

        }

        // EXIT THE WALKING STATE

    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            this.path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }


    #endregion

}


public enum ENEMY_STATE
{
    IDLE,
    WALK,
    ATTACK
}