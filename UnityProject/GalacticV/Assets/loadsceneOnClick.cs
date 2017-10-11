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
        play.onClick.AddListener(() => LoadByIndex());
    }
    // Use this for initialization
    public void LoadByIndex(){
        Debug.Log(index);
		SceneManager.LoadScene (index);
	}
}
