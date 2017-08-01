using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUnlockWeapon : Building {
    
	
	// Update is called once per frame
	public override void FixedUpdate () {
        base.FixedUpdate();	
	}

    public override void ShowGUI()
    {
        GameHUDManager.instance.ShowInfoBox(this, "Unlock Weapon", "This is an example text.");
    }


}
