using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUDManager : MonoBehaviour {

    public static GameHUDManager instance;

    public Slider baseEnergy;

    public Text health;
    public Text energy;
    public Text metal;
    public Text notification;

    private EntityPlayer player;

    public BuildingMenuItem buildingElement;

    public GameObject infoBox;
    public Text infoBoxTitle;
    public Text infoBoxContent;
    private Building buildingInfoBox;

    public GameObject buildMenu;
    private bool showBuildMenu = false;

    public Text MiningPopup;

    public int seenOre=0;

	// Use this for initialization
	void Start () {
        instance = this;


        energy.text = "Energy: 100";
        health.text = "HP: 100";
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
        float healthN = player.health/player.maxHealth * 100;
        health.text = "HP: " + Mathf.Round(healthN);
        metal.text = Mathf.Round(GameManager.instance.currentMetal)+"";

        if(seenOre >0)
        seenOre--;

        if (seenOre > 0)
        {
            MiningPopup.gameObject.SetActive(true);
        } else
        {
            MiningPopup.gameObject.SetActive(false);
        }
    }



    public void sendNotification(string text)
    {
        notification.text = text;
        notification.CrossFadeAlpha(1, 2,false);
        notification.CrossFadeAlpha(0, 2, false);
    }

    public void ShowInfoBox(Building building, string title, string content)
    {
        this.buildingInfoBox = building;
        infoBox.SetActive(true);
        infoBoxTitle.text = title;
        infoBoxContent.text = content;
    }

    public void ButtonInfoBoxClose()
    {
        infoBox.SetActive(false);
        buildingInfoBox = null;
    }

    public void ButtonInfoBoxDestroy()
    {
        infoBox.SetActive(false);
        buildingInfoBox.Dead();
    }

    public void toggleBuildMenu()
    {
        showBuildMenu = !showBuildMenu;
        buildMenu.SetActive(showBuildMenu);
    }
    
}
