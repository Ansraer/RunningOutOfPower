using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityLiving {

    public bool isActive = true;

    private Rigidbody2D rb2d;


	// Use this for initialization
	public override void Awake () {
        base.Awake();

        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	public override void FixedUpdate () {
        base.FixedUpdate();




        if (isActive)
        {
            this.LookAtMouse();
            this.Move();
        }
	}

    private void Move()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxisRaw("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);


        //this.gameObject.transform.Translate(movement * (speed * 0.5f));
        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        rb2d.velocity=(movement*this.GetMovementSpeed());
    }

    private void LookAtMouse()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        //controler stuff
        float moveHorizontal = Input.GetAxisRaw("LookAxisHorizontal");
        float moveVertical = Input.GetAxisRaw("LookAxisVertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 lookVector = new Vector2(moveHorizontal, moveVertical);


        if (moveHorizontal != 0 || moveVertical != 0)
        {
            angle = Mathf.Atan2(moveHorizontal, moveVertical) * Mathf.Rad2Deg + 180;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }
}
