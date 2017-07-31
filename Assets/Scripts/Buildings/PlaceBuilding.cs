using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaceBuilding : MonoBehaviour {

    public GameObject buildingToPlace;

    public Color blockedColor;
    public Color canBuildColor;

    public float metalCost;
    public float energyCost;

    public int placeScore = 10;

    public SpriteRenderer buildingSprite;


    List<Collider2D> triggerList = new List<Collider2D>();
    bool isBlocked = false;

	// Use this for initialization
	void Awake () {
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
            Place();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            
            Destroy(this.gameObject);
        }
    }


    void Place()
    {

        //don't fire when the ui is beeing interacted with
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            foreach (var go in raycastResults)
            {
                if (go.gameObject.name != "Notification" && go.gameObject.name != "MiningPopup")
                    return;
            }
        }



        //dont do something when the player is clicking an building
        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f);
        if (col != null && col.gameObject.GetComponent<Building>() != null)
            return;



        if (GameManager.instance.SpendResources(this.energyCost, this.metalCost))
        {
            Instantiate(buildingToPlace, this.gameObject.transform.position, Quaternion.identity);
            GameManager.totalScore += placeScore;
        }


    }

    //called when something enters the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
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
        //if the object is in the list
        if (triggerList.Contains(other))
        {
            //remove it from the list
            triggerList.Remove(other); 
        }
    }

    public void spawnCursor()
    {
        Vector3 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempPos.z = 0f;
        tempPos.x = (float)Math.Round(tempPos.x);
        tempPos.y = (float)Math.Round(tempPos.y);
        Instantiate(this, tempPos, Quaternion.identity);
    }
}
