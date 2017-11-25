﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {

	private GameObject[] character;
	public List<IUnitScript> P1Characters;
	public List<IUnitScript> P2Characters;
	private int index;
	private int count=10;
	private int p1añadidos=0;
	private int p2added=6;
	[SerializeField] private Texture Icon3;
	string[]texts=new string[]{"RANGED    ATTACKER","DEFENDER"};
	string[]health=new string[]{"8","8"};
	string[]atk=new string[]{"4","4"};
	string[]def=new string[]{"1","1"};
	IUnitScript[] unit= new IUnitScript[]{new RangedUnitScript(),new TankUnitScript()};
	Text word;
	Text vida;
	Text attack;
	Text defense;
	Text player;
	Text playerred;
	private void Start(){
		character=new GameObject[transform.childCount];
		//P1Characters = true;

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
		SoundManager.instance.PlayButtonEffect();
		character [index].SetActive (false);

		index--;


		if (index < 0)
			index = character.Length - 1;

		character [index].SetActive (true);
		word.text = texts [index];
		vida.text = health [index];
		attack.text = atk [index];
		defense.text = def [index];
		//player.text = texts [index];

		print (health [index]);
		print (texts [index]);
		print (atk [index]);
		print (def [index]);

	}


	public void ToggleRight(){
		SoundManager.instance.PlayButtonEffect();
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
		SoundManager.instance.PlayButtonEffect();
		//SceneManager.LoadScene ("MainScene");

		//player.text = texts [index];
		//print (texts [index]);
		if (count % 2 ==0)
		{
			//Text player = GameObject.Find("player");

			player = GameObject.Find ("player"+p1añadidos).GetComponent<Text> ();
			player.text=texts[index];
			print (texts [index]);
			//GameObject t = GameObject.Find("RawImage");

			//t.GetComponent<RawImage>().texture = Icon3;
			P1Characters.Add(unit[index]);
			p1añadidos++;
		}
		else if (count % 2!=0)
		{
			playerred = GameObject.Find ("playerred"+p2added).GetComponent<Text> ();
			playerred.text = texts [index];
			print (texts [index]);

			P2Characters.Add(unit[index]);
			p2added++;
			//
		}

		count--;
	}


}