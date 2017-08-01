using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GunControler : MonoBehaviour {

    
    public GameObject muzzle;
    public float energyConsumptionPerShot = 10;
    public ProjectileController projectile;

    public AudioClip fireSound;

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

        //don't fire when a building id beeing placed
        if (UnityEngine.Object.FindObjectOfType<PlaceBuilding>() != null)
            return;

        //don't fire when the ui is beeing interacted with
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            foreach (var go in raycastResults)
            {
                if (go.gameObject.name != "Notification" && go.gameObject.name != "MiningPopup")
                    return;
            }
        }



        //dont do something when the player is clicking an building
        Collider2D col= Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f);
        if (col != null && col.gameObject.GetComponent<Building>() != null)
            return;




        if (Time.time - lastFired > fireRate && player.energy > this.energyConsumptionPerShot)
        {
            lastFired = Time.time;
            if (this.fireSound != null)
                player.GetComponent<AudioSource>().PlayOneShot(this.fireSound, .5f);

            player.energy -= this.energyConsumptionPerShot;
            Debug.Log(player.energy);

            ProjectileController p = Instantiate(projectile, this.muzzle.transform.position, this.transform.rotation);
            p.GetComponent<Rigidbody2D>().AddForce(this.transform.rotation * (Vector2.up * p.projectileSpeed));
        }
    }

}
