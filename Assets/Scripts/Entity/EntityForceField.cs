using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityForceField : Entity {



    // Use this for initialization
    public virtual void Awake()
    {
        this.health = 100;
    }

    // Update is called once per frame
    public override void FixedUpdate () {
		
	}

    public override void Dead()
    {

    }


    public override void TakeDamage(DamageType type, float amount)
    {
        foreach (DamageResistance resistance in this.resistances)
        {
            if (resistance.type == type)
                amount *= (1 - resistance.amount);
        }


        GameManager.instance.currentEnergy -= amount;
    }
}
