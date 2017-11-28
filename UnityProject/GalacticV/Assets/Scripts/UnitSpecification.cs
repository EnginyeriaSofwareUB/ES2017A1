using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpecification : MonoBehaviour {

    public string type;
    public int movementCost = 1;
    public int attackCost = 1;
    public int defendCost = 1;
    public int abilityCost = 1;


    protected Transform parent;
    [SerializeField]
    protected float lifeValue;
    protected float maxLifeValue;
    protected double defenseModifier;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float Life
    {
        get { return lifeValue; }
        set { this.lifeValue = value; }
    }

    public string GetType()
    {
        return this.type;
    }
}
