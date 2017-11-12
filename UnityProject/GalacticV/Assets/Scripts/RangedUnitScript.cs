using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnitScript : IUnitScript
{
    
	// Use this for initialization
	void Start ()
    {
        base.Start(4, 5, 10, 8, 1);
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
            if(!firstUnit && h.transform.tag != "Coverage" && (h.transform.tag != this.transform.tag))
            {
                firstUnit = true;
                GameObject unitToShoot = h.transform.gameObject;
                distance = (unitToShoot.GetComponent<IUnitScript>().GetDestinationPointRay() - origin).magnitude;
                string nameResource = "Units/Laser" + this.tag;
                GameObject ray = Instantiate(Resources.Load(nameResource)) as GameObject;
                ray.transform.SetParent(this.transform.parent);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                ray.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                ray.transform.position = origin;
                ray.transform.localScale = new Vector3(distance / ray.GetComponent<BoxCollider2D>().size.x, 1, 1);
                Destroy(ray, 0.3f);
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
                    unitToShoot.GetComponent<IUnitScript>().ReduceLife();
                    manager.Tiles[unitToShoot.GetComponent<IUnitScript>().currentPosition].SetColor(Color.white);
                    manager.Tiles[gameController.DestinationUnit.currentPosition].SetColor(Color.white);
                    gameController.DestinationUnit = null;
                }
            }
            else if(!firstUnit && h.transform.tag == "Coverage")
            {
                firstUnit = true;
                GameObject unitToShoot = h.transform.gameObject;
                Vector2 origin2 = new Vector2(origin.x, origin.y);
                distance = (h.point - origin2).magnitude;
                string nameResource = "Units/Laser" + this.tag;
                GameObject ray = Instantiate(Resources.Load(nameResource)) as GameObject;
                ray.transform.SetParent(this.transform.parent);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                ray.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                ray.transform.position = origin;
                ray.transform.localScale = new Vector3(distance / ray.GetComponent<BoxCollider2D>().size.x, 1, 1);
                Destroy(ray, 0.3f);
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
            }
        }
        gameController.SetAbility(" ");
        gameController.ActualCell.SetColor(Color.white);
        gameController.ActualCell = null;
        gameController.ActualUnit.SetSelected(false);
        gameController.ActualUnit = null;
        gameController.SetCancelAction(false);
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
}
