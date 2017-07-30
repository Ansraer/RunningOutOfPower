using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public abstract class Building : MonoBehaviour {

    public float maxHealth=1000;
    public float health;
    public bool activated;

    public float idlePowerUsage=2;
    public float activePowerUsage=3;
    public float bootupEnergy = 30;

	// Use this for initialization
	public virtual void Awake () {
        GameManager.buildings.Add(this);

        this.health = maxHealth;
        this.activated = false;
    }

    // Update is called once per frame
    public virtual void FixedUpdate () {
        if (this.health <= 0)
            this.Destroyed();
	}

    public virtual float GetPowerConsumption()
    {
        return this.activated ? this.activePowerUsage : this.idlePowerUsage;
    }


    public virtual void Destroyed()
    {
        Destroy(this);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) {
            this.ShowGUI();
        } else if(Input.GetMouseButtonDown(1)) {
            this.SwitchState();
        }
    }

    public abstract void ShowGUI();

    public virtual void SwitchState()
    {
        this.activated = !this.activated;
        if(this.activated)
            GameManager.instance.currentEnergy -= this.bootupEnergy;
    }
}
