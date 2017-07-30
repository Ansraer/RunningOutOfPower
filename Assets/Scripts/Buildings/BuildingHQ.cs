using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHQ : Building {
    
	
	// Update is called once per frame
	public override void FixedUpdate () {
        base.FixedUpdate();	
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
