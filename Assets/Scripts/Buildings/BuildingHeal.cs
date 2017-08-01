using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHeal : Building {
    
	
	// Update is called once per frame
	public override void FixedUpdate () {
        base.FixedUpdate();	
	}

    public override void ShowGUI()
    {
        GameHUDManager.instance.ShowInfoBox(this, "Heal", "This is an example text.");
    }


    public override void SwitchState()
    {
        EntityPlayer p = UnityEngine.Object.FindObjectOfType<EntityPlayer>();

        if (p != null)
        {
            float usedEnergy = p.maxHealth - p.health;

            if (GameManager.instance.SpendResources(usedEnergy * 3, 0))
                p.health = p.maxHealth;


        }


    }

}
