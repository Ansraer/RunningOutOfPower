using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHQ : Building {

	// Use this for initialization
	public override void Start () {
		
	}
	
	// Update is called once per frame
	public override void FixedUpdate () {
		
	}

    public override void ShowGUI()
    {
        Debug.Log("showing gui");
    }

    public override void SwitchState()
    {
        Debug.Log("activating");


        this.activated = !this.activated;
    }
}
