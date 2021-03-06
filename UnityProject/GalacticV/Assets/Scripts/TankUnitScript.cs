﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TankUnitScript : IUnitScript {

    void Start()
    {
        base.Start(1, 6, 5, 150, 0.75, "tank");
        this.movementCost = 1;
        this.attackCost = 1;
        this.defendCost = 1;
        this.abilityCost = 5;
    }

    public override void Attack()
    {
        this.GetComponent<Animator>().SetTrigger("attack");
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        manager.DamageInRangeForTank(gameController.destinationPoint, attackRange, this.attackValue);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        gameController.SetAbility(" ");
        gameController.ActualCell.SetColor(Color.white);
        gameController.ActualCell = null;
        gameController.ActualUnit.SetSelected(false);
        gameController.ActualUnit = null;
        gameController.SetCancelAction(false);
        this.GetComponent<Animator>().SetTrigger("idle");
        manager.ClearCurrentRange();
        gameController.FinishAction();
    }

    public override void AttackAction()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        gameController.SetAbility("Attack");
        manager.ShowRange(this.currentPosition, attackRange);
        gameController.SetCancelAction(true);
    }

    public override void CancelAction(string actualAction)
    {
        switch(actualAction)
        {
            case "Move":
                CancelMoveAction();
                break;
            case "Attack":
                CancelAttack();
                break;
            case "Special":
                CancelSpecialHability();
                break;
        }
    }

    public override void CancelAttack()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        gameController.DestinationUnit = null;
        gameController.SetAbility(" ");
        manager.ClearCurrentRange();
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        gameController.SetCancelAction(false);
    }

    public override Vector3 GetDestinationPointRay()
    {
        Vector3 origin;
        if (transform.tag == "Blue")
        {
            origin = (this.transform.position + Vector3.right * 0.25f) + new Vector3(0, 0.6f, 0);
        }
        else
        {
            origin = (this.transform.position + Vector3.left * 0.25f) + new Vector3(0, 0.6f, 0);
        }
        return origin;
    }

    public override Vector3 GetOriginRay()
    {
        throw new NotImplementedException();
    }

    public new void OnMouseOver()
    {
		base.OnMouseOver();
	}

    public override void SpecialHabilityAction()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        gameController.SetAbility("Special");
        manager.ShowRange(this.currentPosition, 1);
        gameController.SetCancelAction(true);
    }

    public override void UseAbility()
    {

    }

    public override void CancelSpecialHability()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        gameController.SetAbility(" ");
        manager.ClearCurrentRange();
        gameController.SetCancelAction(false);
    }
}
