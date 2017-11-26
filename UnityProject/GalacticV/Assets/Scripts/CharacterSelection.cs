using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {

	private GameObject[] character;

	public List<IUnitScript> P1Characters;
	public List<IUnitScript> P2Characters;
	//public List<IUnitScript> units;
	private int index;
	private int count=10;
	private int p1añadidos=0;
	private int p2added=6;
	//[SerializeField] private Texture Icon3;
	//string[]texts=new string[]{"RANGED    ATTACKER","DEFENDER"};
	//string[]health=new string[]{"8","20"};
	string[]atk=new string[]{"4","4"};
	string[]def=new string[]{"1","1"};

	Text word;
	Text vida;
	Text attack;
	Text defense;
	Text player;
	Text playerred;
	private void Start(){
		character=new GameObject[transform.childCount];
		//P1Characters = true;
		//P1Characters=new List<IUnitScript>();
		//P2Characters = new List<IUnitScript> ();

		for (int i = 0; i < transform.childCount; i++) {
			character [i] = transform.GetChild (i).gameObject;
			word = GameObject.Find ("name").GetComponent<Text> ();
			vida = GameObject.Find ("health").GetComponent<Text> ();
			attack = GameObject.Find ("attack").GetComponent<Text> ();
			defense = GameObject.Find ("defense").GetComponent<Text> ();
			word.text=character[index].GetComponent<IUnitScript>().GetType();
			vida.text=character [index].GetComponent<IUnitScript> ().Life.ToString ();

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
		word.text=character[index].GetComponent<IUnitScript>().GetType();
		vida.text=character [index].GetComponent<IUnitScript> ().Life.ToString ();
		attack.text = atk [index];
		defense.text = def [index];
		//player.text = texts [index];

		print(vida.text);
		print (word.text);
		print (attack.text);
		print (defense.text);

	}


	public void ToggleRight(){
		SoundManager.instance.PlayButtonEffect();
		character [index].SetActive (false);

		index++;


		if (index == character.Length)
			index = 0;

		character [index].SetActive (true);
		//word.text = texts [index];
		//vida.text = health [index];
		word.text=character[index].GetComponent<IUnitScript>().GetType();
		vida.text=character [index].GetComponent<IUnitScript> ().Life.ToString ();
		//attack.text = character [index].GetComponent<IUnitScript> ().Life.ToString ();
		attack.text = atk [index];
		//defense.text = def [index];
		defense.text=character[index].GetComponent<IUnitScript>().GetDefenseModifier.ToString();

		print(vida.text);
		print (word.text);
		print (attack.text);

		//print (def [index]);
		print (character[index].GetComponent<IUnitScript>().GetDefenseModifier.ToString());

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
			player.text=character[index].GetComponent<IUnitScript>().GetType();
			print (character[index].GetComponent<IUnitScript>().GetType());


			IUnitScript blueUnit=character[index].GetComponent<IUnitScript>();
			//GameObject t = GameObject.Find("RawImage");

			//t.GetComponent<RawImage>().texture = Icon3;
			P1Characters.Add(blueUnit);

			p1añadidos++;
		}
		else if (count % 2!=0)
		{
			playerred = GameObject.Find ("playerred"+p2added).GetComponent<Text> ();
			playerred.text = character[index].GetComponent<IUnitScript>().GetType();
			print (character[index].GetComponent<IUnitScript>().GetType());
			IUnitScript redUnit=character[index].GetComponent<IUnitScript>();
			P2Characters.Add (redUnit);
			p2added++;
			//
		}

		count--;
	}

	public void playbutton(){
		SoundManager.instance.PlayButtonEffect ();
		SceneManager.LoadScene ("MainScene");

	}

}