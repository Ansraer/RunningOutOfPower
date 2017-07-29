using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class buildBuilding : MonoBehaviour {

    public GameObject buildingPre;
    public GameObject building;

	// Use this for initialization
	void Start () {
        Vector3 initPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        initPos.z = 0f;
        initPos.x = (float)Math.Floor(initPos.x);
        initPos.y = (float)Math.Floor(initPos.y);
        this.building = Instantiate(buildingPre, initPos, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempPos.z = 0f;
        tempPos.x = (float)Math.Floor(tempPos.x);
        tempPos.y = (float)Math.Floor(tempPos.y);
        this.building.transform.position = tempPos;	
	}
}
