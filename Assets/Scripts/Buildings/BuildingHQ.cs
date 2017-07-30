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


}
