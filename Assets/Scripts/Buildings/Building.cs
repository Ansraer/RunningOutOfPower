using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public abstract class Building : MonoBehaviour {

    public float maxHealth;
    public float health;
    public bool activated;


	// Use this for initialization
	public virtual void Start () {
        this.health = maxHealth;
        this.activated = false;
    }

    // Update is called once per frame
    public virtual void FixedUpdate () {
		
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
    public abstract void SwitchState();
}
