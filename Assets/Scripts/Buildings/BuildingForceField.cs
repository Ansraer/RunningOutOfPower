using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingForceField : Building {
    
	
	// Update is called once per frame
	public override void FixedUpdate () {
        base.FixedUpdate();	
	}

    public override void ShowGUI()
    {
        GameHUDManager.instance.ShowInfoBox(this, "Force Field Upgrade", "This is an example text.");
    }
    public override void SwitchState()
    {

    }

}
