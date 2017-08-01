using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStrongPowerplant : Building {
    
	
	// Update is called once per frame
	public override void FixedUpdate () {
        base.FixedUpdate();	
	}

    public override void ShowGUI()
    {
        GameHUDManager.instance.ShowInfoBox(this, "Strong Powerplant", "This is an example text.");
    }


}
