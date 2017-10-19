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
    private GameController gameController;
    
	// Use this for initialization
	void Start ()
    {
        gameController = GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>();
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
        if(gameController.GetHability() != "Move")
        {
            MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
            if (gameController.ActualUnit != null && gameController.ActualUnit != this)
            {
                gameController.ActualCell.PaintUnselected();
                gameController.ActualUnit.isSelected = false;
            }

            if (!isSelected)
            {
                //This is needed because the script is inside another game object
                isSelected = true;
                gameController.ActualUnit = this;
                gameController.ActualCell = manager.Tiles[this.currentPosition];
                gameController.ActualCell.PaintSelected();
                //manager.ShowRange(this.currentPosition, this.movementRange, this);
            }
            else
            {
                isSelected = false;
                gameController.ActualCell.PaintUnselected();
                gameController.ActualUnit = null;
                gameController.ActualCell = null;
                //manager.ClearCurrentRange();
            }
        } 
    }

    public int GetMovementRange()
    {
        return this.movementRange;
    }

    public void SetSelected(bool _selected)
    {
        isSelected = _selected;
    }

    public void MoveTo(Point point, Vector3 worldPos)
    {
        this.currentPosition = point;
        transform.position = worldPos;
        this.isSelected = false;
        gameController.SetCancelAction(false);
    }
}
