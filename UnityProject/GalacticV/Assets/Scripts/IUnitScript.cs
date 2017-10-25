﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUnitScript : MonoBehaviour
{

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
    internal void Start(int attackRange, int movementRange, double attackValue,
                         double lifeValue, double defenseModifier)
    {
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

    public abstract void OnMouseOver();

    public void OnMouseExit()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        if (gameController.GetHability() == "Attack" && gameController.ActualUnit != this)
        {
            manager.Tiles[this.currentPosition].SetColor(Color.white);
        }
    }

    public void OnMouseDown()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        switch (gameController.GetHability())
        {
            case "Move":
                break;
            case "Attack":
                if (this.team != gameController.ActualUnit.team)
                {
                    gameController.DestinationUnit = this;
                    gameController.ActualUnit.Attack();
                }
                break;
            default:
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
                }
                else
                {
                    isSelected = false;
                    gameController.ActualCell.PaintUnselected();
                    gameController.ActualUnit = null;
                    gameController.ActualCell = null;
                }
                break;
        }
    }

    public void MoveAction()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        gameController.SetAbility("Move");
        manager.ShowRange(this.currentPosition, movementRange);
        gameController.SetCancelAction(true);
    }

    public void CancelMoveAction()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        gameController.SetAbility(" ");
        manager.ClearCurrentRange();
        gameController.SetCancelAction(false);
    }
    public abstract void CancelAction(string actualAction);

    public abstract void AttackAction();

    public abstract void Attack();

    public abstract void CancelAttack();
}
