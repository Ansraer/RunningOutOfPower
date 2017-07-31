using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControler : MonoBehaviour {

    public EntityPlayer player;

    public GameObject muzzle;

    public ProjectileController projectile;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Fire()
    {
        ProjectileController p = Instantiate(projectile, this.muzzle.transform.position, this.transform.rotation);

        p.GetComponent<Rigidbody2D>().AddForce(this.transform.rotation * (Vector2.up * p.projectileSpeed));
    }

}
