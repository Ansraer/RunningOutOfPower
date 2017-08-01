using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRevive : Building {


    public float reviveCost = 400f;
	
	// Update is called once per frame
	public override void FixedUpdate () {
        base.FixedUpdate();	
	}

    public override void ShowGUI()
    {
        GameHUDManager.instance.ShowInfoBox(this, "PowerPlant", "This is an example text.");
    }


    public override void SwitchState()
    {
        EntityPlayer p = UnityEngine.Object.FindObjectOfType<EntityPlayer>();

        if (p != null)
        {
            if (p.health <= 0)
            {



                if (GameManager.instance.SpendResources(reviveCost, 0))
                {
                    p.health = p.maxHealth;
                    p.gameObject.SetActive(true);


                }


            }




        }


    }

}
