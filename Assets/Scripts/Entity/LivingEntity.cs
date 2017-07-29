using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour {

    public List<EntityEffect> activeEffects;

    public float maxHealth = 100;
    public float health;

    public float healthRegenPerSecond = 0f; 


	// Use this for initialization
	void Start () {
        this.health = this.maxHealth;
	}
	
	// Update is called once per engine frame
	void FixedUpdate () {

        //kill it if health is 0 or below
        if (this.health <= 0)
            this.Dead();


        //apply health regen
        this.health += this.healthRegenPerSecond * Time.deltaTime;


        foreach (EntityEffect ef in this.activeEffects)
        {
            ef.update(this, Time.deltaTime);
        }
	}

    public void Dead()
    {

        this.health = 0;
        Destroy(this.gameObject);
    }

}
