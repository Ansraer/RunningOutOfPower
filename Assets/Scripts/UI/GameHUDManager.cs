﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUDManager : MonoBehaviour {

    public Slider baseEnergy;

    public Text health;
    public Text energy;
    public Text notification;

    public Slider healthSlider;
    public Slider energySlider;

    private EntityPlayer player;

	// Use this for initialization
	void Start () {
        energy.text = "Energy: 100";
        energySlider.value = 1;
        health.text = "HP: 100";
        healthSlider.value = 1;
        sendNotification("Hi");


        player = Object.FindObjectOfType<EntityPlayer>();
    }

    private void Awake()
    {
        player = Object.FindObjectOfType<EntityPlayer>();
    }

    // Update is called once per frame
    void Update () {
        baseEnergy.value = GameManager.instance.currentEnergy / GameManager.instance.maxEnergy;
        float healthN = player.health/player.maxHealth;
        healthSlider.value = healthN;
        health.text = "HP: " + player.health.ToString();
	}

    public void sendNotification(string text)
    {
        notification.text = text;
        notification.CrossFadeAlpha(1, 2,false);
        notification.CrossFadeAlpha(0, 2, false);
    }
    
}