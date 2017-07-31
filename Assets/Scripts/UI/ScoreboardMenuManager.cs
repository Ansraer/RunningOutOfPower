using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardMenuManager : MonoBehaviour {

    public GameObject scoreboardPanel;
    public GameObject namePanel;
    public Slider loadingBar;

    public Text inputName;

    private bool nameSet = false;
    private bool boardLoaded = false;

    private bool loadingBarGrowing = true;
    public float loadingBarStep = 0.02f;

    dreamloLeaderBoard dl;

    // Use this for initialization
    void Start () {

        // get the reference here...
        this.dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();

        scoreboardPanel.SetActive(false);
        namePanel.SetActive(true);
        loadingBar.gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
        if (nameSet)
        {
            if (loadingBarGrowing)
            {
                loadingBar.value += loadingBarStep;
            } else
            {
                loadingBar.value -= loadingBarStep;
            }


            if(loadingBar.value >= 1)
            {
                loadingBarGrowing = !loadingBarGrowing;
                loadingBar.SetDirection(Slider.Direction.RightToLeft, true);
            }
            else if (loadingBar.value <= 0)
            {
                loadingBarGrowing = !loadingBarGrowing;
                loadingBar.SetDirection(Slider.Direction.LeftToRight, true);
            }


            List<dreamloLeaderBoard.Score> scoreList = dl.ToListHighToLow();


            if (scoreList != null && scoreList.Count>0)
            {

                nameSet = false;
                boardLoaded = true;
                this.loadingBar.gameObject.SetActive(false);
                this.scoreboardPanel.SetActive(true);



                int maxToDisplay = 20;
                int count = 0;
                foreach (dreamloLeaderBoard.Score currentScore in scoreList)
                {

                    count++;

                    Debug.Log(currentScore.playerName +" has a score of "+currentScore.score.ToString());
                    //GUILayout.BeginHorizontal();
                    //GUILayout.Label(currentScore.playerName, width200);
                    //GUILayout.Label(currentScore.score.ToString(), width200);
                    //GUILayout.EndHorizontal();

                    if (count >= maxToDisplay) break;
                }

            }
        }



	}

    public void ButtonSubmitName()
    {
        string name = this.inputName.text;

        if (name.Length < 3)
            return;

        if (dl.publicCode == "") Debug.LogError("You forgot to set the publicCode variable");
        if (dl.privateCode == "") Debug.LogError("You forgot to set the privateCode variable");

        dl.AddScore(name, GameManager.totalScore);

        nameSet = true;

        this.namePanel.SetActive(false);
        this.loadingBar.gameObject.SetActive(true);
    }
}
