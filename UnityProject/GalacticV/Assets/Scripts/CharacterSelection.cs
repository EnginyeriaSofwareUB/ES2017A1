﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {

	private GameObject[] character;

	public List<UnitSpecification> P1Characters;
	public List<UnitSpecification> P2Characters;
	//public List<IUnitScript> units;
	private int index;
	private int count=10;
	private int p1añadidos=0;
	private int p2added=6;
	//[SerializeField] private Texture Icon3;
	//string[]texts=new string[]{"RANGED    ATTACKER","DEFENDER"};
	//string[]health=new string[]{"8","20"};
	//string[]atk=new string[]{"4","4", "4"};
	//string[]def=new string[]{"1","1", "1"};

	Text word;
	Text vida;
	Text attack;
	Text defense;
	Text player;
	Text playerred;
	GameObject buttonplay;
	private void Start(){
		character=new GameObject[transform.childCount];

		P1Characters=new List<UnitSpecification>();
	    P2Characters = new List<UnitSpecification> ();

		for (int i = 0; i < transform.childCount; i++) {
			character [i] = transform.GetChild (i).gameObject;
			word = GameObject.Find ("name").GetComponent<Text> ();
			vida = GameObject.Find ("health").GetComponent<Text> ();
			attack = GameObject.Find ("attack").GetComponent<Text> ();
			defense = GameObject.Find ("defense").GetComponent<Text> ();
			word.text=character[index].GetComponent<UnitSpecification>().GetType();
			vida.text=character [index].GetComponent<UnitSpecification> ().Life.ToString ();
			buttonplay = GameObject.Find ("ButtonPlay");
			attack.text = character[index].GetComponent<UnitSpecification>().attackvalue.ToString();
			defense.text = character[index].GetComponent<UnitSpecification>().GetDefenseModifier.ToString();


		}


		//buttonplay.SetActive (false);

			
		foreach (GameObject go in character) {
			go.SetActive (false);
		}

		if (character [0])
			character [0].SetActive (true);

		buttonplay.SetActive(false);

	}

	public void ToggleLeft(){
		SoundManager.instance.PlayButtonEffect ();
		character [index].SetActive (false);

		index--;


		if (index < 0 && count % 2 == 0) {
			index = character.Length - 5;

		}
		else if (count % 2 != 0) {
			if (index < 4) {
				index = character.Length - 1;
			}

		}

	

		character [index].SetActive (true);
		word.text=character[index].GetComponent<UnitSpecification>().GetType();
		vida.text=character [index].GetComponent<UnitSpecification> ().Life.ToString ();
		//attack.text = atk [index];
		//defense.text = def [index];
		//player.text = texts [index];
		attack.text = character[index].GetComponent<UnitSpecification>().attackvalue.ToString();
		defense.text=character[index].GetComponent<UnitSpecification>().GetDefenseModifier.ToString();

		print(vida.text);
		print (word.text);
		print (attack.text);
		print (defense.text);

	}


	public void ToggleRight(){
		SoundManager.instance.PlayButtonEffect();
		character [index].SetActive (false);

		index++;


		if (index > 3 && count % 2 == 0)
			index = 0;
		else if (count % 2 != 0) {
			if (index == character.Length) {
				index = 4;
			}
		}

		character [index].SetActive (true);
		//word.text = texts [index];
		//vida.text = health [index];
		word.text=character[index].GetComponent<UnitSpecification>().GetType();
		vida.text=character [index].GetComponent<UnitSpecification> ().Life.ToString ();
		//attack.text = character [index].GetComponent<IUnitScript> ().Life.ToString ();
		attack.text = character[index].GetComponent<UnitSpecification>().attackvalue.ToString();
		//defense.text = def [index];
		defense.text=character[index].GetComponent<UnitSpecification>().GetDefenseModifier.ToString();

		print(vida.text);
		print (word.text);
		print (attack.text);

		print (defense.text);
		//print (character[index].GetComponent<IUnitScript>().GetDefenseModifier.ToString());

	}

	public void okbutton(){
		SoundManager.instance.PlayButtonEffect();
        //SceneManager.LoadScene ("MainScene");

        //player.text = texts [index];
        //print (texts [index]);
        if (count > 0)
        {
            if (count % 2 == 0)
            {
               
                player = GameObject.Find("player" + p1añadidos).GetComponent<Text>();
                player.text = character[index].GetComponent<UnitSpecification>().GetType();
                print(character[index].GetComponent<UnitSpecification>().GetType());


                UnitSpecification blueUnit = character[index].GetComponent<UnitSpecification>();
                
                P1Characters.Add(blueUnit);

                p1añadidos++;
				character [index].SetActive (false);
				word.text = " ";
				print (word.text);
				index = 4;
				character [index].SetActive (true);
				word.text=character[index].GetComponent<UnitSpecification>().GetType();
				vida.text=character [index].GetComponent<UnitSpecification> ().Life.ToString ();
				//attack.text = atk [index];
				//defense.text = def [index];
				//player.text = texts [index];
				attack.text = character[index].GetComponent<UnitSpecification>().attackvalue.ToString();
				defense.text=character[index].GetComponent<UnitSpecification>().GetDefenseModifier.ToString();

				print(vida.text);
				print (word.text);
				print (attack.text);
				print (defense.text);

            }
            else if (count % 2 != 0)
            {
                playerred = GameObject.Find("playerred" + p2added).GetComponent<Text>();
                playerred.text = character[index].GetComponent<UnitSpecification>().GetType();
                print(character[index].GetComponent<UnitSpecification>().GetType());
                UnitSpecification redUnit = character[index].GetComponent<UnitSpecification>();
                P2Characters.Add(redUnit);
                p2added++;

				//delete current unit info and show unit of another color
				character [index].SetActive (false);
				word.text = " ";
				print (word.text);
				index = 0;
				character [index].SetActive (true);


				word.text=character[index].GetComponent<UnitSpecification>().GetType();
				vida.text=character [index].GetComponent<UnitSpecification> ().Life.ToString ();

				attack.text = character[index].GetComponent<UnitSpecification>().attackvalue.ToString();
				defense.text=character[index].GetComponent<UnitSpecification>().GetDefenseModifier.ToString();

				print(vida.text);
				print (word.text);
				print (attack.text);
				print (defense.text);
                
            }
        }
        else playbutton();
		count--;


		//show play button when player selection is complete


		if (count % 2 == 0) {
			buttonplay.SetActive (true);
		} else {
			buttonplay.SetActive (false);
		}



	}

	public void playbutton(){

        SoundManager.instance.PlayButtonEffect ();
        List<string> blueUnits = new List<string>();
        foreach(UnitSpecification s in P1Characters)
        {
            blueUnits.Add(s.gameObject.name);
        }
        List<string> redUnits = new List<string>();
        foreach(UnitSpecification s in P2Characters)
        {
            redUnits.Add(s.gameObject.name);
        }
        GameObject.Find("InfoSelection").gameObject.GetComponent<InfoSelection>().SetBlueUnits(blueUnits);
        GameObject.Find("InfoSelection").gameObject.GetComponent<InfoSelection>().SetRedUnits(redUnits);
        DontDestroyOnLoad(GameObject.Find("InfoSelection").gameObject);
		SceneManager.LoadScene ("MainScene");

	}

}