using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class TimeController : MonoBehaviour {

    private float timeRemaining; //In seconds
    private bool countdownActivate;
    private int mana;
    private int manaBuffer;

	[SerializeField]
    private Text timerText;
	[SerializeField]
	private Text manaText;
	[SerializeField]
	private Button surrenderButton;
	[SerializeField]
    private Button passButton;
	[SerializeField]
    public int TIME;


    //Team red sprites
    private Sprite surrenderRed;
	private Sprite surrenderClickedRed;
	private Sprite passRed;
	private Sprite passClickedRed;

	//Team blue sprites
	private Sprite surrenderBlue;
	private Sprite surrenderClickedBlue;
	private Sprite passBlue;
	private Sprite passClickedBlue;

	bool player1Turn;
    bool player2Turn;
    float round;

    // Use this for initialization
	void Start () {
		Init();
		StartTime();
		ChangeColors();
	}

	// Update is called once per frame
	void Update () {
        if (countdownActivate)
        {
            PrintTime();
            timeRemaining -= Time.deltaTime; //Decrement
            if (timeRemaining <= 0) //if over
            {
                EndTurn();
            }
        }
	}

	private void Init()
	{
		player1Turn = true;
		player2Turn = false;
		round = 1;
        mana = 1;
		surrenderRed = Resources.Load<Sprite>("HUD/surrender_red");
		surrenderClickedRed = Resources.Load<Sprite>("HUD/surrender_red_clicked");
		passRed = Resources.Load<Sprite>("HUD/pass_red");
		passClickedRed = Resources.Load<Sprite>("HUD/pass_red_clicked");

		surrenderBlue = Resources.Load<Sprite>("HUD/surrender_blue");
		surrenderClickedBlue = Resources.Load<Sprite>("HUD/surrender_blue_clicked");
		passBlue = Resources.Load<Sprite>("HUD/pass_blue");
		passClickedBlue = Resources.Load<Sprite>("HUD/pass_blue_clicked");
	}

	//Start Time
	private void StartTime()
    {
        countdownActivate = true;
        timeRemaining = TIME;
        PrintMana();
    }

    //Stop Timer
    private void StopTime()
    {
        countdownActivate = false;
        timeRemaining = 0;
    }

    //Print Time with 2 decimals and select color text depending player turn
    private void PrintTime()
    {
        int seconds, minutes;
        minutes = (int) timeRemaining / 60;
        seconds = (int) timeRemaining % 60;
        string niceTime = string.Format("{00:00}:{1:00}", minutes, seconds);
        timerText.text = niceTime;
    }

    //Change Turn of players
    private void ChangeTurn()
    {
        player1Turn = !player1Turn;
        player2Turn = !player2Turn;
		ChangeColors();
	}

	private void ChangeColors()
	{
		//Background color
		Image parent = timerText.transform.parent.GetComponent<Image>();
		parent.color = player1Turn ? Color.blue : Color.red;

		//Surrender button
		surrenderButton.GetComponent<Image>().sprite = player1Turn ? surrenderBlue : surrenderRed;
		//clicked
		SpriteState spriteStateSurrender = surrenderButton.spriteState;
		spriteStateSurrender.pressedSprite = player1Turn ? surrenderClickedBlue : surrenderClickedRed;
		surrenderButton.spriteState = spriteStateSurrender;

		//Pass button
		passButton.GetComponent<Image>().sprite = player1Turn ? passBlue : passRed;
		//clicked
		SpriteState spriteStatePass = passButton.spriteState;
		spriteStatePass.pressedSprite = player1Turn ? passClickedBlue : passClickedRed;
		passButton.spriteState = spriteStatePass;
	}

	//Function called when player end turn
	public void EndTurn()
    {
        if (countdownActivate)
        {
            StartTime();
            ChangeTurn();
            round += 0.5f;
            mana = Mathf.Min((int)round, 10);
            PrintMana();
        }
    }

    // Function called to print mana
    public void PrintMana()
    {
        manaText.text = string.Format("{00:00}", mana) + " / 10";
    }

    //Function called when player surrender
    public void Surrender()
    {
        StopTime();
        //Provisional, es informativo
        string surrenderText = "00:00";
        surrenderText += player1Turn ? "\n\nPlayer 1 Surrended" : "\n\nPlayer 2 Surrended";
        timerText.text = surrenderText;
        StartCoroutine(EndGame());
    }

    public void UseMana()
    {
        mana -= manaBuffer;
        manaBuffer = 0;
        if (mana <= 0) EndTurn();
        else PrintMana();
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene(0);
    }

    // Function called when player pause the game
    public void Pause()
    {
        countdownActivate = !countdownActivate;
    }

    public bool HasEnoughMana(int cost)
    {
        return (mana - manaBuffer) >= cost;
    }

    public void PrepareMana(int manaToUse)
    {
        manaBuffer = manaToUse;

        //Quick workaround to visualize the mana
        mana -= manaBuffer;
        PrintMana();
        mana += manaBuffer;
    }

    public void ReleaseMana()
    {
        manaBuffer = 0;
        PrintMana();
    }

    // Return true/false player 1 turn
    public bool isPlayer1Turn()
    {
        return player1Turn;
    }

    // Return true/false player 2 turn
    public bool isPlayer2Turn()
    {
        return player2Turn;
    }
}
