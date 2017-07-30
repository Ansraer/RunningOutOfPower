using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlaceBuilding : MonoBehaviour {

    public GameObject buildingToPlace;

    public Color blockedColor;
    public Color canBuildColor;

    public SpriteRenderer buildingSprite;


    List<Collider2D> triggerList = new List<Collider2D>();
    bool isBlocked = false;

	// Use this for initialization
	void Start () {
        Vector3 initPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        initPos.z = 0f;
        initPos.x = (float)Math.Floor(initPos.x);
        initPos.y = (float)Math.Floor(initPos.y);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempPos.z = 0f;
        tempPos.x = (float)Math.Round(tempPos.x);
        tempPos.y = (float)Math.Round(tempPos.y);
        this.gameObject.transform.position = tempPos;

        if (this.triggerList.Count <= 0)
        {
            this.buildingSprite.color = this.canBuildColor;
        } else
        {
            this.buildingSprite.color = this.blockedColor;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(buildingToPlace, tempPos, Quaternion.identity);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            
            Destroy(this.gameObject);
        }
    }


    //called when something enters the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger enter");
        //if the object is not already in the list
        if (!triggerList.Contains(other))
        {
            //add the object to the list
            triggerList.Add(other);
        }
    }

    //called when something exits the trigger
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger left");
        //if the object is in the list
        if (triggerList.Contains(other))
        {
            //remove it from the list
            triggerList.Remove(other); 
        }
    }
}
