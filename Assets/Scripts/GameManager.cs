﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GunControler[] allWeapons;
    public static GunControler[] unlockedWeapons = new GunControler[0];


    public float defaultForceFieldRadius = 9;

    public float forceFieldRadius = 9;
    public bool forceFieldActive = false;




    public float minWaveBreakTime = 20f;
    public float maxWaveBreakTime = 45f;

    [System.Serializable]
    public struct SpawnChances
    {
        public GameObject entity;
        public float chance;
    }


    public Transform[] spawnPoints;
    public SpawnChances[] enemies;

    public int currentWave = 0;


    public float maxEnergy = 1000;
    public float currentEnergy = 0;

    public float currentMetal;

    public static List<Building> buildings = new List<Building>();
    public static int totalScore = 0;

    public float startTime = 0;
    public static float gameTime = 0;

    public EntityPlayer deadPlayer { get; internal set; }


    // Use this for initialization
    void Start () {
        Debug.Log("Starting Game");
        instance = this;

        currentEnergy = maxEnergy;

        totalScore = 10;
        startTime = Time.time;

        //update power level every second
        InvokeRepeating("updateBuildingEffects", 0, 1);

        StartCoroutine("EnemyWaveManager");
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (currentEnergy <= 0)
            GameOver();
     }



    //starts a new Coroutine once the old one ends
    IEnumerator EnemyWaveManager()
    {

        yield return new WaitForSeconds(maxWaveBreakTime*3);


        if (spawnPoints.Count() < 1)
        {
            Debug.LogError("Warning, there are not enough spawn points. Please add more transforms to the array.");
            yield return null;
            yield break;
        }

        while (true)
        {
            currentWave++;



            int waveSize = Mathf.FloorToInt(8 + Mathf.Pow(currentWave, 1.6f) * 1.5f);


            float totalSum = 0;

            foreach(SpawnChances chance in this.enemies)
            {
                totalSum += chance.chance;
            }


            for (int i = 0; i < waveSize; i++)
            {

                GameHUDManager.instance.sendNotification("WARNING : ENEMIES INCOMING");
                int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Count()-1);




                float index = UnityEngine.Random.Range(0, totalSum);
                float sum = 0;
                int enemyIndex = -1;

                while (sum < totalSum)
                {
                    enemyIndex++;
                    sum = sum + enemies[enemyIndex].chance;
                }


                Instantiate(enemies[enemyIndex].entity, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.4f, 0.6f));
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(minWaveBreakTime, maxWaveBreakTime));
        }
    }





    internal void AddResources(ItemResources type, float amount)
    {
        currentMetal += amount;
    }

    private void updateBuildingEffects()
    {

        int currentlyUnlockedWeaponsCount = 1;
        float newForceFieldRadiusMultiply = 1;

        List<Building> remove = new List<Building>();

        foreach (Building b in buildings)
        {
            // energy consumption
            currentEnergy -= b.GetPowerConsumption();

            if (b == null) {
                remove.Add(b);
                continue;
            }

            if (b.gameObject.GetComponent<BuildingForceField>() != null)
                newForceFieldRadiusMultiply++;

            if (b.gameObject.GetComponent<BuildingUnlockWeapon>() != null)
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

        if (this.forceFieldRadius < this.defaultForceFieldRadius * newForceFieldRadiusMultiply)
        {
            this.forceFieldRadius = this.defaultForceFieldRadius * newForceFieldRadiusMultiply;

            BuildingHQ hq = UnityEngine.Object.FindObjectOfType<BuildingHQ>();

            if (hq != null)
                hq.changeForceFieldRadius();
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
        gameTime = Time.time - startTime;

        SceneManager.LoadScene("SceneMenuScoreboard");
    }

    public enum ItemResources
    {
        METAL
    }
}
