﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TankUnitScript : IUnitScript {

    void Start()
    {
        base.Start(2, 2, 2, 20, 2);
    }

    public override void Attack()
    {
        
    }

    public override void AttackAction()
    {
        gameController.SetAbility("Attack");
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
        gameController.DestinationUnit = null;
        gameController.SetAbility(" ");
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

    public override void OnMouseOver()
    {
        if (gameController.GetHability() == "Attack")
        {
            MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
            if (!manager.Tiles[currentPosition].GetIsEmpty() && gameController.ActualUnit.team == this.team && gameController.ActualUnit != this)
            {
                manager.Tiles[this.currentPosition].SetColor(Color.red);
            }
            else if (!manager.Tiles[currentPosition].GetIsEmpty() && gameController.ActualUnit.team != this.team)
            {
                manager.Tiles[this.currentPosition].SetColor(Color.green);
            }
        }
    }

    public override void SpecialHabilityAction()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        gameController.SetAbility("Special");
        manager.ShowRange(this.currentPosition, 1);
        gameController.SetCancelAction(true);
    }

    public override void CancelSpecialHability()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        gameController.SetAbility(" ");
        manager.ClearCurrentRange();
        gameController.SetCancelAction(false);
    }
}
