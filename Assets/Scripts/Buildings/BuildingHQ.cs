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
        changeForceFieldRadius();
        this.forceField.SetActive(this.activated);
        GameManager.instance.forceFieldActive = this.activated;
        GameManager.instance.AstarRescan();
    }

    public void changeForceFieldRadius()
    {
        this.forceField.transform.localScale = new Vector3(GameManager.instance.forceFieldRadius*2, GameManager.instance.forceFieldRadius*2, 1);
    }

    public override void Dead()
    {
        base.Dead();
        GameManager.instance.GameOver();
    }

}
