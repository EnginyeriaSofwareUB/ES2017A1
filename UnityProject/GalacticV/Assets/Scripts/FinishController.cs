using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishController : MonoBehaviour {

    private TimeController timeController;

    private bool isGameFinished;
    private bool isGamePaused;

    private GameObject finishMenu;
    private GameObject map;
    private GameObject hud;
    private GameObject pauseButton;
    private GameObject blueText;
    private GameObject redText;
    private PauseController pauseController;

    // Use this for initialization
    void Start () {
        this.isGameFinished = false;
        this.isGamePaused = false;
        timeController = GameObject.FindObjectOfType<TimeController>();
        pauseController = GameObject.FindObjectOfType<PauseController>();
        finishMenu = GameObject.FindGameObjectWithTag("FinishMenu");
        map = GameObject.FindGameObjectWithTag("Map");
        hud = GameObject.FindGameObjectWithTag("Canvas");
        pauseButton = GameObject.FindGameObjectWithTag("PauseButton");
        redText = GameObject.FindGameObjectWithTag("TextRed");
        blueText = GameObject.FindGameObjectWithTag("TextBlue");
        finishMenu.SetActive(isGameFinished);
        
    }
	
	// Update is called once per frame
	void Update () {
        // Section to check if the Blue Team has unit list empty to finish the game.
        if (GameObject.FindGameObjectsWithTag("Blue").Length == 0 && this.isGameFinished == false && this.isGamePaused == false)
        {
            if (timeController.GetIsDelayActivate() == false)
            {
                Debug.Log("Red Team wins.");
                timeController.SetIsDelayActivate(true);
            }
            else
            {
                if (timeController.GetTimeDelayRemaining() <= 0)
                {
                    timeController.SetIsDelayActivate(false);
                    this.isGameFinished = true;
                    finishMenu.SetActive(isGameFinished);
                    pauseController.SetIsGameFinished(isGameFinished);
                    map.SetActive(!isGameFinished);
                    hud.SetActive(!isGameFinished);
                    pauseButton.SetActive(!isGameFinished);
                    blueText.SetActive(false);
                }
            }
        }

        // Section to check if the Blue Team has unit list empty to finish the game.
        if (GameObject.FindGameObjectsWithTag("Red").Length == 0 && this.isGameFinished == false && this.isGamePaused == false)
        {
            if (timeController.GetIsDelayActivate() == false)
            {
                Debug.Log("Blue Team wins.");
                timeController.SetIsDelayActivate(true);
            }
            else
            {
                if (timeController.GetTimeDelayRemaining() <= 0)
                {
                    timeController.SetIsDelayActivate(false);
                    this.isGameFinished = true;
                    finishMenu.SetActive(isGameFinished);
                    pauseController.SetIsGameFinished(isGameFinished);
                    map.SetActive(!isGameFinished);
                    hud.SetActive(!isGameFinished);
                    pauseButton.SetActive(!isGameFinished);
                    redText.SetActive(false);
                }
            }
        }
    }

    public void SetIsGamePaused(bool state)
    {
        this.isGamePaused = state;
    }


}
