using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileController : MonoBehaviour {

    public float projectileSpeed=500;

    public Entity.DamageResistance[] damages;


    public float piercing = 0;

    public bool damagePlayer=false;

    public bool damageBuilding = false;

    public void Awake()
    {
        Invoke("Destroy", 4);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}


    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger)
            return;

        this.piercing--;

        this.HitTarget(other);

        if (piercing < 0)
            Destroy(this.gameObject);


    }

    public virtual void HitTarget(Collider2D other)
    {
        if (other.GetComponent<Entity>() == null)
            return;

        if (other.GetComponent<Building>() != null && !this.damageBuilding)
            return;

        if (other.GetComponent<EntityPlayer>() != null && !this.damagePlayer)
            return;


        foreach (Entity.DamageResistance d in this.damages)
        {
            other.GetComponent<Entity>().TakeDamage(d.type, d.amount);
        }
    }
}
