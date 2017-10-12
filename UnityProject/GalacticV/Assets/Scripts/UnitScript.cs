using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour {

    public Point currentPosition;
    private SpriteRenderer spriteRenderer;
    public int team; //team id
    public bool isSelected = false;
    private int attackRange;
    private int movementRange;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackRange = 5;
        movementRange = 4;
	}

    public void Setup(Point point, Vector3 worldPos, Transform parent)
    {
        this.currentPosition = point;
        transform.position = worldPos;
        transform.SetParent(parent);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnMouseDown()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        if (!isSelected)
        {
            //This is needed because the script is inside another game object
            isSelected = true;
            manager.ShowRange(this.currentPosition, this.movementRange, this);
        }
        else
        {
            isSelected = false;
            manager.ClearCurrentRange();
        }

        
    }

    public void MoveTo(Point point, Vector3 worldPos)
    {
        this.currentPosition = point;
        transform.position = worldPos;
        this.isSelected = false;
    }
}
