using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnitScript : IUnitScript
{

    private int abilityRange = 1;

	// Use this for initialization
	void Start ()
    {
		base.Start(4, 5, 10, 100, 1, "ranged");
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

    public override void AttackAction()
    {
		gameController.SetAbility("Attack");
        gameController.SetCancelAction(true);
    }

    public override void Attack()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
		SoundManager.instance.PlayEffect("Effects/laser_effect_1");
		Vector3 origin = GetOriginRay();
        Vector3 heading = gameController.DestinationUnit.GetDestinationPointRay() - origin;
        float distance = heading.magnitude;
        Vector3 direction = heading / distance;
        int layerMask = 1 << 8;
        bool firstUnit = false;
        layerMask = ~layerMask;
        RaycastHit2D[] hit = Physics2D.RaycastAll(origin, direction, Mathf.Infinity, layerMask);
        foreach(RaycastHit2D h in hit)
        {
            if(!firstUnit && h.transform.tag == ("Coverage"+gameController.DestinationUnit.tag))
            {
                firstUnit = true;
                GameObject unitToShoot = h.transform.gameObject;
                Vector2 origin2 = new Vector2(origin.x, origin.y);
                distance = (h.point - origin2).magnitude;
                string nameResource = "Units/Laser" + this.tag;
                string explosionName = "Objects/ExplosionRed";
                GameObject ray = Instantiate(Resources.Load(nameResource)) as GameObject;
                GameObject expl = Instantiate(Resources.Load(explosionName)) as GameObject;
                ray.transform.SetParent(this.transform.parent);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                ray.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                ray.transform.position = origin;
                ray.transform.localScale = new Vector3(distance / ray.GetComponent<BoxCollider2D>().size.x, 1, 1);
                expl.transform.position = h.transform.position;
                Destroy(ray, 0.3f);
                Destroy(expl, 0.5f);
                if (h.transform.GetComponent<CoverageScript>().IsFull())
                {
                    h.transform.GetComponent<CoverageScript>().ChangeSprite();
                }
                else
                {
                    manager.Tiles[h.transform.GetComponent<CoverageScript>().GridPosition].SetIsEmpty(true);
                    manager.Tiles[h.transform.GetComponent<CoverageScript>().GridPosition].SetColor(Color.white);
                    manager.Tiles[gameController.DestinationUnit.currentPosition].SetColor(Color.white);
                    Destroy(h.transform.gameObject);
                }
                manager.Tiles[gameController.DestinationUnit.currentPosition].SetColor(Color.white);
                gameController.DestinationUnit = null;
                break;
            }
            else if(!firstUnit && (h.transform.tag != "Coverage" && h.transform.tag != ("Coverage"+gameController.ActualUnit.tag)) && (h.transform.tag != this.transform.tag))
            {
                firstUnit = true;
                GameObject unitToShoot = h.transform.gameObject;
                distance = (unitToShoot.GetComponent<IUnitScript>().GetDestinationPointRay() - origin).magnitude;
                string nameResource = "Units/Laser" + this.tag;
                string explosionName = "Objects/ExplosionRed";
                GameObject ray = Instantiate(Resources.Load(nameResource)) as GameObject;
                GameObject expl = Instantiate(Resources.Load(explosionName)) as GameObject;
                ray.transform.SetParent(this.transform.parent);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                ray.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                ray.transform.position = origin;
                ray.transform.localScale = new Vector3(distance / ray.GetComponent<BoxCollider2D>().size.x, 1, 1);
                expl.transform.position = h.transform.gameObject.GetComponent<IUnitScript>().GetDestinationPointRay();
                Destroy(ray, 0.3f);
                Destroy(expl, 0.5f);
                if (this.GetAttack >= h.transform.GetComponent<IUnitScript>().Life)
                {
                    manager.Tiles[unitToShoot.GetComponent<IUnitScript>().currentPosition].SetIsEmpty(true);
                    manager.Tiles[unitToShoot.GetComponent<IUnitScript>().currentPosition].SetColor(Color.white);
                    manager.Tiles[gameController.DestinationUnit.currentPosition].SetColor(Color.white);
                    Destroy(h.transform.gameObject);
                    gameController.DestinationUnit = null;
                }
                else
                {
                    unitToShoot.GetComponent<IUnitScript>().Life = unitToShoot.GetComponent<IUnitScript>().Life - this.GetAttack;
                    unitToShoot.GetComponent<IUnitScript>().TakeDamage(this.GetAttack);
                    unitToShoot.GetComponent<IUnitScript>().ReduceLife();
                    manager.Tiles[unitToShoot.GetComponent<IUnitScript>().currentPosition].SetColor(Color.white);
                    manager.Tiles[gameController.DestinationUnit.currentPosition].SetColor(Color.white);
                    gameController.DestinationUnit = null;
                }
                break;
            }
            else if(!firstUnit && h.transform.tag == "Coverage")
            {
                firstUnit = true;
                GameObject unitToShoot = h.transform.gameObject;
                Vector2 origin2 = new Vector2(origin.x, origin.y);
                distance = (h.point - origin2).magnitude;
                string nameResource = "Units/Laser" + this.tag;
                string explosionName = "Objects/ExplosionRed";
                GameObject ray = Instantiate(Resources.Load(nameResource)) as GameObject;
                GameObject expl = Instantiate(Resources.Load(explosionName)) as GameObject;
                ray.transform.SetParent(this.transform.parent);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                ray.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                ray.transform.position = origin;
                ray.transform.localScale = new Vector3(distance / ray.GetComponent<BoxCollider2D>().size.x, 1, 1);
                expl.transform.position = h.transform.position;
                Destroy(ray, 0.3f);
                Destroy(expl, 0.5f);
                if(h.transform.GetComponent<CoverageScript>().IsFull())
                {
                    h.transform.GetComponent<CoverageScript>().ChangeSprite();
                }
                else
                {
                    Destroy(h.transform.gameObject);
                }
                manager.Tiles[gameController.DestinationUnit.currentPosition].SetColor(Color.white);
                gameController.DestinationUnit = null;
                break;
            }
        }
        gameController.SetAbility(" ");
        gameController.ActualCell.SetColor(Color.white);
        gameController.ActualCell = null;
        gameController.ActualUnit.SetSelected(false);
        gameController.ActualUnit = null;
        gameController.SetCancelAction(false);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        gameController.FinishAction();
    }

    public override void CancelAttack()
    {
        gameController.DestinationUnit = null;
        gameController.SetAbility(" ");
        gameController.SetCancelAction(false);
    }

    public override Vector3 GetOriginRay()
    {
        Vector3 origin;
        if(transform.tag == "Blue")
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


    public override void UseAbility()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        manager.DamageInRange(gameController.destinationPoint, abilityRange, this.attackValue);
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
}
