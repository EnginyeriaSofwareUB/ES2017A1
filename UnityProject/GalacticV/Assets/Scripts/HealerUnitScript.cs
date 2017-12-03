using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HealerUnitScript : IUnitScript
{

	private int abilityRange = 1;

	// Use this for initialization
	void Start()
	{
		base.Start(1, 5, 5, 100, 1, "healer");
		this.movementCost = 1;
		this.attackCost = 2;
		this.defendCost = 1;
		this.abilityCost = 3;
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
		}
	}

	public override void Attack()
	{
		this.GetComponent<Animator>().SetTrigger("attack");
		MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
		IUnitScript unit = gameController.DestinationUnit;
		if (attackValue >= unit.Life)
		{
			manager.Tiles[unit.currentPosition].SetColor(Color.white);
			manager.Tiles[unit.currentPosition].SetIsEmpty(true);
			gameController.DestinationUnit = null;
			Destroy(unit.transform.gameObject);
		}
		else
		{
			unit.Life -= (float)attackValue;
			unit.ReduceLife();
			manager.Tiles[unit.currentPosition].SetColor(Color.white);
		}
		gameController.SetAbility(" ");
		gameController.ActualCell.SetColor(Color.white);
		gameController.ActualCell = null;
		gameController.ActualUnit.SetSelected(false);
		gameController.ActualUnit = null;
		gameController.SetCancelAction(false);
		manager.ClearCurrentRange();
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		gameController.FinishAction();
		this.GetComponent<Animator>().SetTrigger("idle");
	}

	public override void AttackAction()
	{
		gameController.SetAbility("Attack");
		MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
		manager.ShowRange(this.currentPosition, attackRange);
		gameController.SetCancelAction(true);
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
		MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
		gameController.DestinationUnit.Life = Mathf.Min(gameController.DestinationUnit.Life + 10, gameController.DestinationUnit.GetMaxLifeValue);
		gameController.DestinationUnit.ReduceLife();
		gameController.SetAbility(" ");
		gameController.ActualCell.SetColor(Color.white);
		gameController.ActualCell = null;
		gameController.ActualUnit.SetSelected(false);
		gameController.ActualUnit = null;
		gameController.SetCancelAction(false);
		gameController.FinishAction();
	}

	public override void SpecialHabilityAction()
	{
		gameController.SetAbility("Ability");
		gameController.SetCancelAction(true);
	}

	public override void CancelSpecialHability()
	{
		gameController.SetAbility(" ");
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
}
