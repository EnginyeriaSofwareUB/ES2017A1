using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour {

    public Point currentPosition;
    private SpriteRenderer spriteRenderer;
    public int team; //team id
    public bool isSelected = false;
    private GameController gameController;
    /*Player attributes*/
    private int movementRange;
    private int healthPoints;
    private int attackDamage;
    private int defensePoints;
    private int attackRange;


    // Use this for initialization
    void Start ()
    {
        gameController = GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackRange = 5;
        movementRange = 4;
        healthPoints = 200;
        attackDamage = 50;
        defensePoints = 20;
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
                gameController.ShowPlayerStats();
                
                //infopanel.GetComponent<InfoPanel>();
                //manager.ShowRange(this.currentPosition, this.movementRange, this);
            }
            else
            {
                isSelected = false;
                gameController.ActualCell.PaintUnselected();
                gameController.ActualUnit = null;
                gameController.ActualCell = null;
				gameController.HidePlayerStats();
                //manager.ClearCurrentRange();
            }
        } 
    }

    #region Player Attribute Getters
    public int GetMovementRange()
    {
        return this.movementRange;
    }

    public int GetHealthPoints()
    {
        return this.healthPoints;
    }

    public int GetAttackDamage()
    {
        return this.attackDamage;
    }

    public int GetDefensePoints()
    {
        return this.defensePoints;
    }
    #endregion

    public SpriteRenderer GetSpriteRenderer()
    {
        return spriteRenderer;
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
