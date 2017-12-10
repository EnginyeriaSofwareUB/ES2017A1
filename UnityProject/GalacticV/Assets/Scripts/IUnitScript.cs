using Assets.Scripts;
using Assets;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUnitScript : MonoBehaviour
{

    public Point currentPosition;
    private SpriteRenderer spriteRenderer;
    public int team; //team id
    public string type;
    public int movementCost = 1;
    public int attackCost = 1;
    public int defendCost = 1;
    public int abilityCost = 1;

    public bool isSelected = false;
    protected int attackRange;
    protected int movementRange;
    protected double attackValue;
    [SerializeField]
    protected Enums.UnitState state = Enums.UnitState.Idle;

    //movement values
    protected const float speed = 5f;
    protected Vector3 targetTransform;
    protected List<Vector3> vectorPath;
    protected Point targetPosition;
    protected Transform parent;
    [SerializeField]
    protected float lifeValue;
	protected float maxLifeValue;
    protected double defenseModifier;
    protected GameController gameController;

    public float Life
    {
        get { return lifeValue; }
        set { this.lifeValue = value; }
    }

	public float GetMaxLifeValue
	{
		get { return maxLifeValue; }
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
                         float lifeValue, double defenseModifier, string type)
    {
        gameController = GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.attackRange = attackRange;
        this.movementRange = movementRange;
        this.attackValue = attackValue;
        this.lifeValue = lifeValue;
		this.maxLifeValue = lifeValue;
        this.defenseModifier = defenseModifier;
        this.type = type;
    }

    public void Setup(Point point, Vector3 worldPos, Transform parent)
    {
        this.currentPosition = point;
        transform.position = worldPos;
        this.parent = parent;
        transform.SetParent(parent);
    }

    public string GetType()
    {
        return this.type;
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
        //SoundManager.instance.PlayEffect("Effects/walk_effect_2.1", true);
        this.GetComponent<Animator>().SetTrigger("move");
        this.transform.parent = parent;
        this.isSelected = false;
        gameController.SetCancelAction(false);
        this.state = Enums.UnitState.Move;
        this.targetPosition = point;
        this.vectorPath = vectorPath;
        this.targetTransform = this.vectorPath.First();
        this.vectorPath.Remove(this.targetTransform);
	}

    public void OnMouseOver()
	{
		MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
		switch (gameController.GetHability())
		{
			case "Attack":

				if (!manager.Tiles[currentPosition].GetIsEmpty() && gameController.ActualUnit.team == this.team && gameController.ActualUnit != this)
				{
					manager.Tiles[this.currentPosition].SetColor(Color.red);
				}
				else if (!manager.Tiles[currentPosition].GetIsEmpty() && gameController.ActualUnit.team != this.team)
				{
					manager.Tiles[this.currentPosition].SetColor(Color.green);
				}
				break;
			case "Ability":
				if (!manager.Tiles[currentPosition].GetIsEmpty())//si la cela es lliure
				{
					if (gameController.ActualUnit.type == "healer")//en el cas que sigui healer
					{
						if (gameController.ActualUnit.team != this.team )
						{
							manager.Tiles[this.currentPosition].SetColor(Color.red);
						}
						else if (gameController.ActualUnit.team == this.team && gameController.ActualUnit != this)
						{
							manager.Tiles[this.currentPosition].SetColor(Color.yellow);
						}
					}else{
						if (gameController.ActualUnit.team != this.team && gameController.ActualUnit != this)
						{
							manager.Tiles[this.currentPosition].SetColor(Color.red);
						}
						else if (gameController.ActualUnit.team == this.team)
						{
							manager.Tiles[this.currentPosition].SetColor(Color.yellow);
						}
					}
				}


				break;
		}
	}

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
				if (this.team != gameController.ActualUnit.team && gameController.ActualUnit.type == "ranged")
				{
					gameController.DestinationUnit = this;
					gameController.ActualUnit.Attack();
					gameController.HidePlayerStats();
				}
                else if (this.team != gameController.ActualUnit.team && gameController.ActualUnit.type == "tank" || gameController.ActualUnit.type == "melee")
                {
					gameController.destinationPoint = this.currentPosition;
					gameController.ActualUnit.Attack();
					gameController.HidePlayerStats();
				}
                else if (this.team != gameController.ActualUnit.team && gameController.ActualUnit.type == "healer")
				{
					if (manager.Distance(gameController.ActualUnit.currentPosition, this.currentPosition) < gameController.ActualUnit.GetAttack + 1)
					{
						gameController.DestinationUnit = this;
						gameController.ActualUnit.Attack();
						gameController.HidePlayerStats();
					}
				}


				break;
			case "Ability":
				if (gameController.ActualUnit.type == "healer" && this.team == gameController.ActualUnit.team && gameController.ActualUnit != this) {
					gameController.DestinationUnit = this;
					gameController.ActualUnit.UseAbility();
				}
				else if (gameController.ActualUnit.type != "healer")
				{
					gameController.destinationPoint = this.currentPosition;
					gameController.ActualUnit.UseAbility();
					gameController.HidePlayerStats();
				}
                break;
            case "Special":
                if (gameController.ActualUnit.GetType() != "melee") return;
                if (gameController.ActualUnit.team == this.team) return;
                gameController.DestinationUnit = this;
                gameController.ActualUnit.UseAbility();
                gameController.HidePlayerStats();
                break;
            default:
                if (this.state != Enums.UnitState.Defense)
                {
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

    public void DeffenseAction()
    {
        this.state = Enums.UnitState.Defense;
        string teamUnit = this.team == 0 ? "Blue" : "Red";
        GameObject shield  = this.gameObject.transform.Find("HealthCanvas").transform.Find("Shield" + teamUnit).gameObject;
        this.defenseModifier /= 2;
        shield.SetActive(true);
        gameController.SetAbility(" ");
        gameController.ActualCell.SetColor(Color.white);
        gameController.ActualCell = null;
        gameController.ActualUnit.SetSelected(false);
        gameController.ActualUnit = null;
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
                        this.GetComponent<Animator>().SetTrigger("idle");
                        gameController.MakeInteractableButtons(false);
                        this.currentPosition = this.targetPosition;
                        //SoundManager.instance.StopEffect();
                        gameController.FinishAction();
                    }
                }
                break;
            case Enums.UnitState.Skill:
                step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetTransform, step);
                if (transform.position == targetTransform)
                {
                    state = Enums.UnitState.Idle;
                    this.GetComponent<Animator>().SetTrigger("idle");
                    this.currentPosition = this.targetPosition;
                    //SoundManager.instance.StopEffect();
                    gameController.FinishAction();
                }
                break;
            default:
                break;
        }
    }

	public List<int> GetCostActions()
	{
		return new List<int>() {movementCost, attackCost, defendCost, abilityCost };
	}

	public Enums.UnitState GetState()
	{
		return state;
	}
    public abstract void CancelAction(string actualAction);

    public abstract void AttackAction();

    public abstract void Attack();

    public abstract void CancelAttack();

    public abstract void UseAbility();

    public abstract Vector3 GetOriginRay();

	public abstract Vector3 GetDestinationPointRay();

    public abstract void SpecialHabilityAction();

    public abstract void CancelSpecialHability();

    public void ReduceLife()
    {
        GameObject bar = gameObject.transform.GetChild(0).transform.GetChild(1).gameObject;
        if (bar.GetComponent<HealthBar>() != null) bar.GetComponent<HealthBar>().ReduceLife(this.lifeValue);
    }

    public void TakeDamage(double damageValue)
    {
        this.lifeValue -= (float)(damageValue*this.defenseModifier);
    }

    public void SetIdleState()
    {
        this.state = Enums.UnitState.Idle;
    }
}
