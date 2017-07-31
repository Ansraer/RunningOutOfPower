using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GunControler[] allWeapons;
    public static GunControler[] unlockedWeapons = new GunControler[0];



    public float forceFieldRadius = 9;
    public bool forceFieldActive = false;


    public float maxEnergy = 1000;
    public float currentEnergy = 0;

    public float currentMetal;

    public static List<Building> buildings = new List<Building>();
    internal static int totalScore = 0;


    // Use this for initialization
    void Start () {
        Debug.Log("Starting Game");
        instance = this;

        currentEnergy = maxEnergy;

        totalScore = 10;


        //update power level every second
        InvokeRepeating("updateBuildingEffects", 0, 1);

        //generate Map here
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (currentEnergy <= 0)
            GameOver();





	}

    internal void AddResources(ItemResources type, float amount)
    {
        currentMetal += amount;
    }

    private void updateBuildingEffects()
    {

        int currentlyUnlockedWeaponsCount=0;

        List<Building> remove = new List<Building>();

        foreach (Building b in buildings)
        {
            // energy consumption
            currentEnergy -= b.GetPowerConsumption();

            if (b == null) {
                remove.Add(b);
                continue;
            }

            if (b.gameObject.GetComponent<Building>() != null)
                currentlyUnlockedWeaponsCount++;

        }

        foreach (Building rb in remove)
        {
            buildings.Remove(rb);
        }

        //make sure that the number of unlocked weapons is right
        if(unlockedWeapons.Length != currentlyUnlockedWeaponsCount)
        {

            if (currentlyUnlockedWeaponsCount > allWeapons.Length)
                currentlyUnlockedWeaponsCount = allWeapons.Length;

            unlockedWeapons = allWeapons.Take(currentlyUnlockedWeaponsCount).ToArray<GunControler>();

            EntityPlayer[] players = UnityEngine.Object.FindObjectsOfType<EntityPlayer>();

            foreach(EntityPlayer p in players)
            {
                p.SwitchWeapon(0);
            }
        }
    }


    /** tries to spend resources to build something
     *  returns true if the resources are available and have been spen 
     *  returns false if there aren't enough resources
     */
    public bool SpendResources(float energy, float metal)
    {
        if(this.currentEnergy >= energy && this.currentMetal>= metal)
        {
            this.currentEnergy -= energy;
            this.currentMetal -= metal;
            return true;
        }

        return false;
    }

    public void AstarRescan()
    {
        AstarPath.active.Scan();

    }

    public void GameOver()
    {

    }

    public enum ItemResources
    {
        METAL
    }
}
