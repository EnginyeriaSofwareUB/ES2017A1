using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUnitScript : MonoBehaviour {

    public Point currentPosition;
    private SpriteRenderer spriteRenderer;
    public int team; //team id


    public bool isSelected = false;
    protected int attackRange;
    protected int movementRange;
    protected double attackValue;
    protected double lifeValue;
    protected double defenseModifier;
    protected GameController gameController;

    // Use this for initialization
    internal void Start (int attackRange, int movementRange, double attackValue, 
                         double lifeValue, double defenseModifier) {
        gameController = GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.attackRange = attackRange;
        this.movementRange = movementRange;
        this.attackValue = attackValue;
        this.lifeValue = lifeValue;
        this.defenseModifier = defenseModifier;
    }

    public void Setup(Point point, Vector3 worldPos, Transform parent)
    {
        this.currentPosition = point;
        transform.position = worldPos;
        transform.SetParent(parent);
    }


    public int GetMovementRange()
    {
        return this.movementRange;
    }

    public void SetSelected(bool _selected)
    {
        isSelected = _selected;
    }

    public void MoveTo(Point point, Vector3 worldPos)
    {
        this.currentPosition = point;
        transform.position = worldPos;
        this.isSelected = false;
        gameController.SetCancelAction(false);
    }

    public void OnMouseDown()
    {
        if (gameController.GetHability() != "Move")
        {
            MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
            if (gameController.ActualUnit != null && gameController.ActualUnit != this)
            {
                gameController.ActualCell.PaintUnselected();
                gameController.ActualUnit.isSelected = false;
            }

            if (!isSelected)
            {
                //This is needed because the script is inside another game object
                isSelected = true;
                gameController.ActualUnit = this;
                gameController.ActualCell = manager.Tiles[this.currentPosition];
                gameController.ActualCell.PaintSelected();
                manager.ShowRange(this.currentPosition, this.movementRange);
				gameController.ShowPlayerStats();
            }
            else
            {
                isSelected = false;
                gameController.ActualCell.PaintUnselected();
                gameController.ActualUnit = null;
                gameController.ActualCell = null;
                manager.ClearCurrentRange();
				gameController.HidePlayerStats();
            }
        }
    }

	public double GetAttackValue()
	{
		return this.attackValue;
	}

	public double GetLifeValue()
	{
		return this.lifeValue;
	}

	public double GetDefenseModifier()
	{
		return this.defenseModifier;
	}

    // Update is called once per frame
    void Update () {
		
	}

}
