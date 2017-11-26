using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    private IUnitScript actualUnit;
    [SerializeField]
    private CellScript actualCell;
    [SerializeField]
    private IUnitScript destinationUnit;
    public Point destinationPoint;

    [SerializeField]
    private Texture2D cursor;
    private string habilitySelected;
    private bool cancellAction;
    private TimeController timeController;
    
    public IUnitScript ActualUnit
    {
        get { return actualUnit; }
        set { actualUnit = value; }
    }

    public CellScript ActualCell
    {
        get { return actualCell; }
        set { actualCell = value; }
    }

    public IUnitScript DestinationUnit
    {
        get { return destinationUnit; }
        set { destinationUnit = value; }
    }

    // Use this for initialization
    void Start () {
        actualUnit = null;
        actualCell = null;
        habilitySelected = " ";
        cancellAction = false;
        timeController = GameObject.FindObjectOfType<TimeController>();
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        manager.Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (this.checkTurn())
            {
                Move();
            } 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (this.checkTurn())
            {
                Attack();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if(this.checkTurn())
            {
                Deffense();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if(this.checkTurn())
            {
                SpecialHability();
            }
        }
    }

	public void Move()
	{
		if (this.actualUnit != null && habilitySelected != " " && habilitySelected != "Move")
		{
			actualUnit.CancelAction(habilitySelected);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
		if (this.actualUnit != null && !cancellAction)
		{
            if (!timeController.HasEnoughMana(this.actualUnit.movementCost)) return;
			actualUnit.MoveAction();
            timeController.PrepareMana(this.actualUnit.movementCost);
        }
		else if (this.actualUnit != null && cancellAction)
		{
			actualUnit.CancelMoveAction();
            timeController.ReleaseMana();
        }
    }

	public void Attack()
	{
		if (this.actualUnit != null && habilitySelected != " " && habilitySelected != "Attack")
		{
			actualUnit.CancelAction(habilitySelected);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
		if (this.actualUnit != null && !cancellAction)
		{
            if (!timeController.HasEnoughMana(this.actualUnit.attackCost)) return;
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
            this.actualUnit.AttackAction();
            timeController.PrepareMana(this.actualUnit.attackCost);
        }
		else if (this.actualUnit != null && cancellAction)
		{
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            this.actualUnit.CancelAttack();
            timeController.ReleaseMana();
        }
    }

    public void Deffense()
    {
        if (this.actualUnit != null & habilitySelected != " " && habilitySelected != "Special")
        {
            actualUnit.CancelAction(habilitySelected);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        if (this.actualUnit != null && !cancellAction)
        {
            int cost = this.actualUnit.abilityCost;
            if (!timeController.HasEnoughMana(this.actualUnit.defendCost)) return;
            actualUnit.DeffenseAction();
            timeController.PrepareMana(cost);
            FinishAction();
        }
    }

    public void SpecialHability()
    {
        if (this.actualUnit !=  null & habilitySelected != " " && habilitySelected != "Special")
        {
            actualUnit.CancelAction(habilitySelected);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        if (this.actualUnit != null && !cancellAction)
        {
            if (!timeController.HasEnoughMana(this.actualUnit.abilityCost)) return;
            actualUnit.SpecialHabilityAction();
            timeController.PrepareMana(this.actualUnit.abilityCost);
        }
        else if (this.actualUnit != null && cancellAction)
        {
            //cancel special action
            actualUnit.CancelSpecialHability();
            timeController.ReleaseMana();
        }
    }

	public string GetHability()
    {
        return habilitySelected;
    }
    
    public void SetAbility(string s)
    {
        habilitySelected = s;
    }

	public bool IsCancelAction()
	{
		return this.cancellAction;
	}

    public void SetCancelAction(bool _cancelAction)
    {
        cancellAction = _cancelAction;
    }

    public void FinishAction()
    {
		timeController.UseMana();
		HidePlayerStats();
	}

    public void ShowPlayerStats()
    {
        if (this.checkTurn()) {
            InfoPanelScript infoPanel = GameObject.FindGameObjectWithTag("Canvas").
                                    transform.Find("InfoPanel").GetComponent<InfoPanelScript>();
            infoPanel.ShowPanel(ActualUnit);
        }
    }

	public void HidePlayerStats()
	{
		InfoPanelScript infoPanel = GameObject.FindGameObjectWithTag("Canvas").
			transform.Find("InfoPanel").GetComponent<InfoPanelScript>();
		infoPanel.HidePanel();
	}

    // Return true if we can do actions with the select unit
    public bool checkTurn() {
        if (timeController.isPlayer1Turn())
        {
            if (this.actualUnit.tag == "Blue")
            {
                return true;
            } else
            {
                return false;
            }
        } else {
            if (this.actualUnit.tag == "Red")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

	public int GetMana()
	{
		return timeController.GetMana();
	}

	public int GetManaBuffer()
	{
		return timeController.GetManaBuffer();
	}
}
