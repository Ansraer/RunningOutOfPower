using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControler : MonoBehaviour {

    
    public GameObject muzzle;
    public float energyConsumptionPerShot = 10;
    public ProjectileController projectile;

    public EntityPlayer player;

    public float fireRate = 20;
    private float lastFired=-9999;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

    public void Fire()
    {

        if (Time.time - lastFired > fireRate && player.energy > this.energyConsumptionPerShot)
        {
            lastFired = Time.time;


            player.energy -= this.energyConsumptionPerShot;

            ProjectileController p = Instantiate(projectile, this.muzzle.transform.position, this.transform.rotation);
            p.GetComponent<Rigidbody2D>().AddForce(this.transform.rotation * (Vector2.up * p.projectileSpeed));
        }
    }

}
