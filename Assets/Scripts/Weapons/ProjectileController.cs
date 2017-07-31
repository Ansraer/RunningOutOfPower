using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileController : MonoBehaviour {

    public float projectileSpeed=500;

    public float damage=10;



    // Update is called once per frame
    void Update () {
		
	}


    void OnTriggerEnter2D(Collider2D other)
    {
        




    }
}
