using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour {

	private GameObject[] character;

	private void Start(){
		character=new GameObject[transform.childCount];

		for (int i = 0; i < transform.childCount; i++) {
			character [i] = transform.GetChild (i).gameObject;
		}

		foreach (GameObject go in character) {
			go.SetActive (false);
		}

		if (character [0])
			character [0].SetActive (true);
		
	}


}
