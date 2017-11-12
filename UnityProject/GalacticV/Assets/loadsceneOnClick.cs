using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadsceneOnClick : MonoBehaviour {

    [SerializeField]
    private int index;
    private void Start()
    {
        Button play = gameObject.GetComponent<Button>();
		if (index >= 0)//hi ha botons que no portaran a cap escena, pero interesa aquest script pel so.
		{
			play.onClick.AddListener(() => LoadByIndex());
		}
        
    }
    // Use this for initialization
    public void LoadByIndex(){
		PlayButtonEffect();
		SceneManager.LoadScene (index);
	}

	public void PlayButtonEffect()
	{
		SoundManager.instance.PlayButtonEffect();
	}

}
