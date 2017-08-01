using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityLiving {

    public bool isActive = true;
    public bool isInteracting;

    public GameObject muzzle;

    public GunControler gun;

    private Rigidbody2D rb2d;

    public float energy;

    public float maxEnergy = 1000;

    private int currentWeapon;

	// Use this for initialization
	public override void Awake () {
        base.Awake();

        this.energy = maxEnergy;

        this.SwitchWeapon(0);

        isInteracting = false;
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	public override void FixedUpdate () {
        base.FixedUpdate();




        if (isActive)
        {
            this.LookAtMouse();
            this.HandleInput();
        }
	}

    private void HandleInput()
    {
        //interact with world
        this.isInteracting = Input.GetButton("Interact");

        //fire
        if (Input.GetButton("Fire1"))
        {
            this.Shoot();
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if(Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                SwitchWeapon(1);
            } else
            {
                SwitchWeapon(-1);
            }
        }





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

    private void Shoot()
    {

        //this.projectile.Spawn(this.muzzle.transform);
        this.gun.Fire();

    }

    public void SwitchWeapon(int i)
    {
        int newWeapon = this.currentWeapon + i;


        if (newWeapon > GameManager.unlockedWeapons.Length -1)
        {
            newWeapon = GameManager.unlockedWeapons.Length - 1;
        }
        if (newWeapon < 0)
        {
            newWeapon = 0;

        }

        if ((newWeapon != currentWeapon || this.gun==null) && GameManager.unlockedWeapons.Length>0)
        {

            if(this.gun!=null)
                Destroy(this.gun.gameObject);

            GunControler g = Instantiate(GameManager.unlockedWeapons[newWeapon], this.transform.position + this.transform.rotation * GameManager.unlockedWeapons[newWeapon].transform.position, this.transform.rotation);
            g.gameObject.transform.parent = this.gameObject.transform;
            g.player = this;
            this.gun = g;

            this.currentWeapon = newWeapon;
        }
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

    public override void Dead()
    {

        this.health = 0;
        this.gameObject.SetActive(false);
    }


}
