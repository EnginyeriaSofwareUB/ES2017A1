using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnitScript : IUnitScript {

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
        
    }

    public override void AttackAction()
    {
       
    }

    public override void Attack()
    {
        
    }

    public override void CancelAttack()
    {
        
    }
    
    public override void UseAbility()
    {

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
        throw new NotImplementedException();
    }

    public override void CancelSpecialHability()
    {
        throw new NotImplementedException();
    }
}
