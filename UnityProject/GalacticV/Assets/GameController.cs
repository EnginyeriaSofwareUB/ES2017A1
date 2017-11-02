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
    private string habilitySelected;
    private bool cancellAction;
    
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
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        manager.Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
			Move();
		}
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
			Attack();
        }
    }

	public void Move()
	{
		if (this.actualUnit != null && habilitySelected != " " && habilitySelected != "Move")
		{
			actualUnit.CancelAction(habilitySelected);
		}
		if (this.actualUnit != null && !cancellAction)
		{
			actualUnit.MoveAction();
		}
		else if (this.actualUnit != null && cancellAction)
		{
			actualUnit.CancelMoveAction();
		}
	}

	public void Attack()
	{
		if (this.actualUnit != null && habilitySelected != " " && habilitySelected != "Attack")
		{
			actualUnit.CancelAction(habilitySelected);
		}
		if (this.actualUnit != null && !cancellAction)
		{
			this.actualUnit.AttackAction();
		}
		else if (this.actualUnit != null && cancellAction)
		{
			this.actualUnit.CancelAttack();
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

    public void ShowPlayerStats()
    {
        InfoPanelScript infoPanel = GameObject.FindGameObjectWithTag("Canvas").
                                    transform.Find("InfoPanel").GetComponent<InfoPanelScript>();
        infoPanel.ShowPanel(ActualUnit);
    }

	public void HidePlayerStats()
	{
		InfoPanelScript infoPanel = GameObject.FindGameObjectWithTag("Canvas").
			transform.Find("InfoPanel").GetComponent<InfoPanelScript>();
		infoPanel.HidePanel();
	}
}
