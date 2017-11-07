using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {

    private bool showPause;

    private GameObject pauseMenu;
    private GameObject map;
    private GameObject hud;

	// Use this for initialization
	void Start () {
        showPause = false;
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        map = GameObject.FindGameObjectWithTag("Map");
        hud = GameObject.FindGameObjectWithTag("Canvas");
        pauseMenu.SetActive(showPause);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    // Function called when player pause the game
    public void Pause()
    {
        showPause = !showPause;
        pauseMenu.SetActive(showPause);
        map.SetActive(!showPause);
        hud.SetActive(!showPause);
  
    }

    // Function called when player press resume game button on pause menu
    public void ResumeGame()
    {
        Pause();
    }

    // Function called when player press quit game button on pause menu
    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}
