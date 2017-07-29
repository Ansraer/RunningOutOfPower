using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMainManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ButtonPlay()
    {
        SceneManager.LoadScene("SceneGameMain");
    }

    public void ButtonAchievements()
    {

    }

    public void ButtonQuit()
    {
        Application.Quit();
    }
}
