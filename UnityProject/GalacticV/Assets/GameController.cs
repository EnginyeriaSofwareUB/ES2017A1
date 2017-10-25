using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    private UnitScript actualUnit;
    [SerializeField]
    private CellScript actualCell;
    private string habilitySelected;
    private bool cancellAction;
    
    public UnitScript ActualUnit
    {
        get { return actualUnit; }
        set { actualUnit = value; }
    }

    public CellScript ActualCell
    {
        get { return actualCell; }
        set { actualCell = value; }
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
            if(this.actualUnit != null && !cancellAction)
            {
                habilitySelected = "Move";
                MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
                manager.ShowRange(this.actualUnit.currentPosition, this.actualUnit.GetMovementRange());
                cancellAction = true;
            }
            else if(actualUnit != null && cancellAction)
            {
                habilitySelected = "";
                MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
                manager.ClearCurrentRange();
                cancellAction = false;
            }

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
