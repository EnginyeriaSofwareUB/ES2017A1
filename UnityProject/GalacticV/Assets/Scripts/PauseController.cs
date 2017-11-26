using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {

    private bool showPause;
    private bool isGameFinished;

    private GameObject pauseMenu;
    private GameObject map;
    private GameObject hud;
    private TimeController timeController;
    private FinishController finishController;

    // Use this for initialization
    void Start () {
        showPause = false;
        isGameFinished = false;
        timeController = GameObject.FindObjectOfType<TimeController>();
        finishController = GameObject.FindObjectOfType<FinishController>();
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        map = GameObject.FindGameObjectWithTag("Map");
        hud = GameObject.FindGameObjectWithTag("Canvas");
        pauseMenu.SetActive(showPause);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameFinished == false)
        {
            timeController.Pause();
            Pause();
        }
    }

    // Function called when player pause the game
    public void Pause()
    {
        //SoundManager.instance.PlayButtonEffect();
        showPause = !showPause;
        finishController.SetIsGamePaused(showPause);
        pauseMenu.SetActive(showPause);
        map.SetActive(!showPause);
        hud.SetActive(!showPause);
    }

    // Function called when player press resume game button on pause menu
    public void ResumeGame()
    {
		//SoundManager.instance.PlayButtonEffect();
		timeController.Pause();
        Pause();
    }

    // Function called when player press quit game button on pause menu
    public void QuitGame()
    {
		//SoundManager.instance.PlayButtonEffect();
		SceneManager.LoadScene(0);
    }

    public void SetIsGameFinished (bool state)
    {
        this.isGameFinished = state;
    }
}
