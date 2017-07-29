using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{


    public ENEMY_STATE state;


    public GameObject defaultTarget;


    //how close the enemy has to be to attack
    public float maxAttackDistance =5;

    //how close something has to be to become the new primary target
    public float maxDetectionDistance = 6;



    private GameObject target;


    #region UNITY METHODS

    public void Awake()
    {
        state = ENEMY_STATE.IDLE;
    }

    public void Start()
    {
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
            EntityPlayer[] players = Object.FindObjectsOfType<EntityPlayer>();



            foreach(EntityPlayer p in players)
            {
                if(Vector3.Distance(this.transform.position, p.transform.position) <= this.maxDetectionDistance)
                {
                    Debug.Log("now targeting player");
                    state = ENEMY_STATE.WALK;
                    this.target = p.gameObject;
                    yield break;

                }
            }



            yield return null;
        }

        // EXIT THE IDLE STATE

    }


    IEnumerator WALK()
    {

        // EXECUTE IDLE STATE
        while (state == ENEMY_STATE.WALK)
        {
            Debug.Log("walking");

            yield return null;
        }

        // EXIT THE WALKING STATE

        //Debug.Log("No longer walking");
    }


    #endregion

}


public enum ENEMY_STATE
{
    IDLE,
    WALK,
    ATTACK
}