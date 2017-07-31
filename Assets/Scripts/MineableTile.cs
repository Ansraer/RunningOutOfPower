using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MineableTile : MonoBehaviour {

    public GameObject minedTile;

    public float resources = 100;
    public float dropAmount = 1;
    public float harvestTime=60;
    public float mineTime;

    public bool isTouchingPlayer = false;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isTouchingPlayer)
        {
            GameHUDManager.instance.seenOre = 5;
        }
        isTouchingPlayer = false;

        if (this.mineTime > this.harvestTime)
            this.Mine();
	}


    void Mine()
    {
        this.mineTime = 0;

        if (this.dropAmount <= this.resources)
        {
            this.resources -= this.dropAmount;
            GameManager.instance.AddResources(GameManager.ItemResources.METAL, this.dropAmount);
        } else
        {
            GameManager.instance.AddResources(GameManager.ItemResources.METAL, this.resources);
            this.resources = 0;
        }




        if (this.resources <= 0)
        {
            Instantiate(minedTile, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer==LayerMask.NameToLayer("Player"))
        {
            this.isTouchingPlayer = true;

            if (other.GetComponent<EntityPlayer>().isInteracting)
            {
                this.mineTime++;
            }

        }
    }
}
