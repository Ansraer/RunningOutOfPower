using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCharge : Building {
    
	
	// Update is called once per frame
	public override void FixedUpdate () {
        base.FixedUpdate();	
	}

    public override void ShowGUI()
    {
        GameHUDManager.instance.ShowInfoBox(this, "Charge", "This is an example text.");
    }

    public override void SwitchState()
    {
        EntityPlayer p = UnityEngine.Object.FindObjectOfType<EntityPlayer>();

        if (p != null)
        {
            float usedEnergy = p.maxEnergy - p.energy;

            if (GameManager.instance.SpendResources(usedEnergy, 0))
                p.energy = p.maxEnergy;


        }


    }

}