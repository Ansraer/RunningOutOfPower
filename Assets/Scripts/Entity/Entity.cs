using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    public float maxHealth = 100;
    public float health;
    public float healthRegenPerSecond = 0f;

    [System.Serializable]
    public struct DamageResistance
    {
        public DamageType type;
        public float amount;
    }


    public DamageResistance[] resistances;

    public List<EntityEffect> activeEffects = new List<EntityEffect>();


    // Use this for initialization
    public virtual void Awake()
    {
        this.health = this.maxHealth;
    }

    // Update is called once per engine frame
    public virtual void FixedUpdate()
    {

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



    public virtual void Dead()
    {

        this.health = 0;
        Destroy(this.gameObject);
    }


    public virtual void TakeDamage(DamageType type, float amount)
    {
        foreach (DamageResistance resistance in this.resistances)
        {
            if (resistance.type == type)
                amount += (1 - resistance.amount);
        }


        this.health -= amount;
    }





    public enum DamageType
    {
        GENERAL, PHYSICAL, ELECTRICITY,
    }
}
