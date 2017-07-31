using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMainManager : MonoBehaviour {

    public Button[] buttonAnimation;

    public Color defaultNormalColor;
    public Color darkNormalColor;

    public float minFlickerSpeed = 1f;
    public float maxFlickerSpeed = 4f;

    // Use this for initialization
	void Start () {

        foreach (Button b in buttonAnimation)
        {
            StartCoroutine("FlickerButton", b);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator FlickerButton(Button b)
    {
        while (true)
        {

            var colors = b.colors;
            colors.normalColor = darkNormalColor;
            b.colors = colors;
            b.GetComponentInChildren<Text>().color = darkNormalColor;

            yield return new WaitForSeconds(Random.Range(minFlickerSpeed/7, maxFlickerSpeed/8));

            Debug.Log("setting colors back to normal");

            colors.normalColor = defaultNormalColor;
            b.colors = colors;
            b.GetComponentInChildren<Text>().color = defaultNormalColor;


            yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
        }
        yield return null;
    }


    public void ButtonPlay()
    {
        SceneManager.LoadScene("SceneGameMain");
    }

    public void ButtonScoreboard()
    {
        SceneManager.LoadScene("SceneMenuScoreboard");

    }

    public void ButtonQuit()
    {
        Application.Quit();
    }
}
