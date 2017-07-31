using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuItem : MonoBehaviour {

    public PlaceBuilding building;

    public Image image;

    public Text metalCost;
    public Text energyCost;



    public void ButtonPlac()
    {
        this.building.spawnCursor();
    }

	// Use this for initialization
	void Start () {
        this.image.sprite = building.GetComponent<SpriteRenderer>().sprite;

        this.metalCost.text = building.metalCost + "";
        this.energyCost.text = building.energyCost + "";
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
