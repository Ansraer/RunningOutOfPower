using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public static List<Building> buildings = new List<Building>();

    public float maxEnergy = 1000;
    public float currentEnergy = 0;


    public float currentMetal;

	// Use this for initialization
	void Start () {
        Debug.Log("Starting Game");
        instance = this;

        currentEnergy = maxEnergy;


        //update power level every second
        InvokeRepeating("updatePower", 0, 1);

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

    private void updatePower()
    {
        foreach (Building b in buildings)
        {
            currentEnergy -= b.GetPowerConsumption();
        }
    }

    public void GameOver()
    {

    }

    public enum ItemResources
    {
        METAL
    }
}
