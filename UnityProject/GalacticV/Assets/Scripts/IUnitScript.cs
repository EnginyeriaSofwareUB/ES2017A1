using Assets.Scripts;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUnitScript : MonoBehaviour
{

    public Point currentPosition;
    private SpriteRenderer spriteRenderer;
    public int team; //team id

    public int movementCost = 1;
    public int attackCost = 1;
    public int defendCost = 1;
    public int abilityCost = 1;

    public bool isSelected = false;
    protected int attackRange;
    protected int movementRange;
    protected double attackValue;
    protected Enums.UnitState state = Enums.UnitState.Idle;

    //movement values
    protected const float speed = 1;
    protected Vector3 targetTransform;
    protected List<Vector3> vectorPath;
    protected Point targetPosition;

    [SerializeField]
    protected double lifeValue;
    protected double defenseModifier;
    protected GameController gameController;

    public double Life
    {
        get { return lifeValue; }
        set { this.lifeValue = value; }
    }

    public int GetAttack
    {
        get { return attackRange; }
        set { this.attackRange = value; }
    }

	public double GetDefenseModifier
	{
		get { return defenseModifier; }
		set { this.defenseModifier = value; }
	}

	// Use this for initialization
	internal void Start(int attackRange, int movementRange, double attackValue,
                         double lifeValue, double defenseModifier)
    {
        gameController = GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.attackRange = attackRange;
        this.movementRange = movementRange;
        this.attackValue = attackValue;
        this.lifeValue = lifeValue;
        this.defenseModifier = defenseModifier;
    }

    public void Setup(Point point, Vector3 worldPos, Transform parent)
    {
        this.currentPosition = point;
        transform.position = worldPos;
        transform.SetParent(parent);
    }


    public int GetMovementRange()
    {
        return this.movementRange;
    }

    public void SetSelected(bool _selected)
    {
        isSelected = _selected;
    }

    public void MoveTo(Point point, List<Vector3> vectorPath)
    {
		//this.currentPosition = point;
		//transform.position = worldPos;
		SoundManager.instance.PlayEffect("Effects/walk_effect_2.1", true);
         this.isSelected = false;
        gameController.SetCancelAction(false);
        this.state = Enums.UnitState.Move;
        this.targetPosition = point;
        this.vectorPath = vectorPath;
        this.targetTransform = this.vectorPath.First();
        this.vectorPath.Remove(this.targetTransform);

    }

    public abstract void OnMouseOver();

    public void OnMouseExit()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        if (gameController.GetHability() == "Attack" && gameController.ActualUnit != this)
        {
            manager.Tiles[this.currentPosition].SetColor(Color.white);
        }
    }

    public void OnMouseDown()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        switch (gameController.GetHability())
        {
            case "Move":
                break;
            case "Attack":
                if (this.team != gameController.ActualUnit.team)
                {
                    gameController.DestinationUnit = this;
                    gameController.ActualUnit.Attack();
					gameController.HidePlayerStats();
				}
                break;
            default:
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
                }
                else
                {
                    isSelected = false;
                    gameController.ActualCell.PaintUnselected();
                    gameController.ActualUnit = null;
                    gameController.ActualCell = null;
					gameController.HidePlayerStats();
                }
                break;
        }
    }

    public void MoveAction()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        gameController.SetAbility("Move");
        manager.ShowRange(this.currentPosition, movementRange);
        gameController.SetCancelAction(true);
    }

    public void CancelMoveAction()
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        gameController.SetAbility(" ");
        manager.ClearCurrentRange();
        gameController.SetCancelAction(false);
    }

    private void Update()
    {
        switch(state)
        {
            case Enums.UnitState.Move:
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetTransform, step);
                if (transform.position == targetTransform) {
                    if (this.vectorPath.Any())
                    {
                        targetTransform = this.vectorPath.First();
                        this.vectorPath.Remove(targetTransform);
                    }
                    else
                    {
                    state = Enums.UnitState.Idle;
                    this.currentPosition = this.targetPosition;
						SoundManager.instance.StopEffect();
                    }
                }
                break;
            default:
                break;
        }
    }

    public abstract void CancelAction(string actualAction);

    public abstract void AttackAction();

    public abstract void Attack();

    public abstract void CancelAttack();

    public abstract Vector3 GetOriginRay();

	public abstract Vector3 GetDestinationPointRay();
}
