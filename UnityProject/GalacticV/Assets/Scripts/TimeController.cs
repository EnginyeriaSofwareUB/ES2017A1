using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour {

    public int TIME;
    private float timeRemaining; //In seconds
    private bool countdownActivate;
    public Text timerText;
    public Text manaText;

    bool player1Turn;
    bool player2Turn;
    float round;

    // Use this for initialization
	void Start () {
        player1Turn = true;
        player2Turn = false;
        round = 1;
        StartTime();
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
        Image parent = timerText.transform.parent.GetComponent<Image>();
        parent.color = player1Turn ? Color.blue : Color.red;
    }

    //Change Turn of players
    private void ChangeTurn()
    {
        player1Turn = !player1Turn;
        player2Turn = !player2Turn;
    }

    //Function called when player end turn
    public void EndTurn()
    {
        if (countdownActivate)
        {
            StartTime();
            ChangeTurn();
            round += 0.5f;
            PrintMana();
        }
    }

    // Function called to print mana
    public void PrintMana()
    {
        int mana = Mathf.Min((int)round, 10);
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
    }
}
