using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {

	private GameObject[] character;
	private int index;
	string[]texts=new string[]{"B L U E","R E D"};
	string[]health=new string[]{"8","8"};
	string[]atk=new string[]{"4","4"};
	string[]def=new string[]{"1","1"};

	Text word;
	Text vida;
	Text attack;
	Text defense;
	private void Start(){
		character=new GameObject[transform.childCount];


		for (int i = 0; i < transform.childCount; i++) {
			character [i] = transform.GetChild (i).gameObject;
			word = GameObject.Find ("name").GetComponent<Text> ();
			vida = GameObject.Find ("health").GetComponent<Text> ();
			attack = GameObject.Find ("attack").GetComponent<Text> ();
			defense = GameObject.Find ("defense").GetComponent<Text> ();
			word.text = texts [index];
			vida.text = health [index];
			attack.text = atk [index];
			defense.text = def [index];

		}



		foreach (GameObject go in character) {
			go.SetActive (false);
		}

		if (character [0])
			character [0].SetActive (true);
		
	}

	public void ToggleLeft(){
		character [index].SetActive (false);

		index--;

	
		if (index < 0)
			index = character.Length - 1;

		character [index].SetActive (true);
		word.text = texts [index];
		vida.text = health [index];
		attack.text = atk [index];
		defense.text = def [index];
		print (health [index]);
		print (texts [index]);
		print (atk [index]);
		print (def [index]);

	}


	public void ToggleRight(){
		character [index].SetActive (false);

		index++;


		if (index == character.Length)
			index = 0;

		character [index].SetActive (true);
		word.text = texts [index];
		vida.text = health [index];
		attack.text = atk [index];
		defense.text = def [index];
		print (health [index]);
		print (texts [index]);
		print (atk [index]);
		print (def [index]);


	}

	public void okbutton(){
		SceneManager.LoadScene ("MainScene");
		
	}


}
