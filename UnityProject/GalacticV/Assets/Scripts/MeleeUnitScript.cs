using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnitScript : IUnitScript {

    float abilityDamage = 15;

    // Use this for initialization
    void Start()
    {
        base.Start(1, 5, 15, 200, 0.5, "melee");
    }


	public new void OnMouseOver()
	{
		base.OnMouseOver();
	}

	public override void CancelAction(string actualAction)
    {
        switch (actualAction)
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

    public override void AttackAction()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        gameController.SetAbility("Attack");
        manager.LineRange(this.currentPosition, 1);
        gameController.SetCancelAction(true);
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
        this.GetComponent<Animator>().SetTrigger("idle");
        gameController.SetCancelAction(false);
        manager.ClearCurrentRange();
        gameController.FinishAction();
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
    
    public override void UseAbility()
    {
        this.targetPosition = gameController.DestinationUnit.currentPosition;
        if (currentPosition.X == targetPosition.X)
        {
            if (currentPosition.Y > targetPosition.Y)
                targetPosition.Y++;
            else targetPosition.Y--;
        }
        else
        {
            if (currentPosition.X > targetPosition.X)
                targetPosition.X++;
            else targetPosition.X--;
        }
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        //targetTransform = manager.GetPosition(targetPosition);
        targetTransform = manager.Tiles[targetPosition].transform.position;
        manager.ClearCurrentRange();
        manager.Tiles[currentPosition].SetIsEmpty(true);
        this.state = Assets.Scripts.Enums.UnitState.Skill;
        this.GetComponent<Animator>().SetTrigger("move");
        gameController.DestinationUnit.TakeDamage(this.abilityDamage);
        gameController.DestinationUnit.ReduceLife();
        gameController.SetAbility(" ");
        gameController.ActualCell.SetColor(Color.white);
        gameController.ActualCell = null;
        gameController.ActualUnit.SetSelected(false);
        gameController.ActualUnit = null;
        gameController.SetCancelAction(false);
        gameController.FinishAction();
    }

    public override Vector3 GetOriginRay()
    {
        Vector3 origin;
        if (transform.tag == "Blue")
        {
            origin = (this.transform.position + Vector3.right * 0.75f) + new Vector3(0, 1.1f, 0);
        }
        else
        {
            origin = (this.transform.position + Vector3.left * 0.75f) + new Vector3(0, 1.1f, 0);
        }
        return origin;
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

    public override void SpecialHabilityAction()
    {
        gameController.SetAbility("Special");
        gameController.SetCancelAction(true);
        var mapManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        mapManager.LineRange(this.currentPosition, 4);
    }

    public override void CancelSpecialHability()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        gameController.SetAbility(" ");
        manager.ClearCurrentRange();
        gameController.SetCancelAction(false);
    }
}
