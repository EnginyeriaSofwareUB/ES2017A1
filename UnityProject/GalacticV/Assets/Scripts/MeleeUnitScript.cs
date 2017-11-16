using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnitScript : IUnitScript {

    // Use this for initialization
    void Start()
    {
        base.Start(1, 5, 15, 200, 0.5);
    }


    public override void OnMouseOver()
    {
        
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

    public override Vector3 GetOriginRay()
    {
        return new Vector3(0,0,0);
    }

    public override Vector3 GetDestinationPointRay()
    {
        return new Vector3(0, 0, 0);
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
