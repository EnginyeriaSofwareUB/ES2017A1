using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnitScript : IUnitScript {
    
	// Use this for initialization
	void Start ()
    {
        base.Start(4, 5, 10, 150, 1);
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

    public override void AttackAction()
    {
        gameController.SetAbility("Attack");
        gameController.SetCancelAction(true);
    }

    public override void Attack()
    {
        //Disparar objecte cap a unitat destination i restejar
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        manager.Tiles[gameController.DestinationUnit.currentPosition].SetColor(Color.white);
        gameController.DestinationUnit = null;
        gameController.SetAbility(" ");
        gameController.ActualCell.SetColor(Color.white);
        gameController.ActualCell = null;
        gameController.ActualUnit.SetSelected(false);
        gameController.ActualUnit = null;
        gameController.SetCancelAction(false);
    }

    public override void CancelAttack()
    {
        gameController.DestinationUnit = null;
        gameController.SetAbility(" ");
        gameController.SetCancelAction(false);
    }
}
