using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHQ : Building {

    public GameObject forceField;


    public override void Awake()
    {
        base.Awake();

        this.forceField.gameObject.SetActive(false);
    }

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
        base.SwitchState();

        this.forceField.SetActive(this.activated);
    }

    public override void Dead()
    {
        base.Dead();
        GameManager.instance.GameOver();
    }

}
