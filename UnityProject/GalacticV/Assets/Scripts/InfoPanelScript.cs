using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelScript : MonoBehaviour
{
	/*Panel objects*/
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private GameObject unitPreview;
    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private GameObject attackBar;
    [SerializeField]
    private GameObject defenseBar;

	#region Action Buttons
	/*Action Buttons*/
	private Button moveButton;
	private Button attackButton;
	private Button defenseButton;
	private Button abilityButton;
	#endregion
	#region Image Buttons
	/*Image Buttons*/
	private Image moveImage;
	private Image attackImage;
	private Image defenseImage;
	private Image abilityImage;
	#endregion
	#region Cost Texts
	/*Cost Texts*/
	private Text moveCost;
	private Text attackCost;
	private Text defenseCost;
	private Text abilityCost;
	#endregion
	#region Sprites
	/*Resources*/
	private Sprite blueBackgroundSprite;
	private Sprite redBackgroundSprite;
	private Sprite attackSpriteRangeUnitBlue;
	private Sprite attackSpriteRangeUnitRed;
	private Sprite attackSpriteCombatBlue;
	private Sprite attackSpriteCombatRed;
	private Sprite attackSpriteTankUnit;
	private Sprite defenseSpriteBlue;
	private Sprite defenseSpriteRed;
	private Sprite abilitySpriteRangeUnit;
	private Sprite abilitySpriteHealerBlue;
	private Sprite abilitySpriteHealerRed;
	#endregion

	/*Runtime attributes*/
	[SerializeField]
	private IUnitScript unit;
	private GameController gameController;

	// Use this for initialization
	void Start()
    {
		
		gameController = GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>();
		LoadResources ();
		InitElements();
		Hide ();
    }

	private void Update()
	{
		if (this.gameObject.activeSelf && unit != null)
		{
			Sprite currentUnitPreviewSprite = this.unitPreview.GetComponent<Image>().sprite;//Sprite Actual de la Preview
			Sprite currentUnitSprite = unit.gameObject.GetComponent<SpriteRenderer>().sprite; //Sprite actual de la unitat
			if (currentUnitPreviewSprite != currentUnitSprite)//Si son diferents, l'actualitzem
			{
				SetTeamSprite(currentUnitSprite);
			}

			if (unit.GetState() == Enums.UnitState.Move)
			{
				moveButton.interactable = false;
				attackButton.interactable = false;
				defenseButton.interactable = false;
				abilityButton.interactable = false;
				SetAlfa(attackImage, 0.5f);
				SetAlfa(defenseImage, 0.5f);
				SetAlfa(moveImage, 0.5f);
				SetAlfa(abilityImage, 0.5f);
			}
		}
	}

	public void ShowPanel(IUnitScript unit)
    {
		this.unit = unit;
		this.gameObject.GetComponent<Image>().sprite = unit.team == 0 ? blueBackgroundSprite : redBackgroundSprite;
		PrintStats(unit);
		UpdateResources (unit);
		PrintCostActions(unit);
		CheckActions(unit);
		Show();
	}

	public void HidePanel()
	{
		Hide();
		this.unit = null;
	}


	private void PrintStats(IUnitScript unit)
    {
		this.healthBar.GetComponent<StatusBar>().UpdateStatusBar((float) unit.Life, unit.GetMaxLifeValue);
		this.attackBar.GetComponent<StatusBar>().UpdateStatusBar(unit.GetAttack, unit.GetAttack);
		this.defenseBar.GetComponent<StatusBar>().UpdateStatusBar((float) unit.GetDefenseModifier, (float)unit.GetDefenseModifier);
	}

	private void PrintCostActions(IUnitScript unit)
	{
		List<int> costActions = unit.GetCostActions();
		this.moveCost.text = costActions[(int)Enums.Actions.Move].ToString();
		this.attackCost.text = costActions[(int)Enums.Actions.Attack].ToString();
		this.defenseCost.text = costActions[(int)Enums.Actions.Defense].ToString();
		this.abilityCost.text = costActions[(int)Enums.Actions.Ability].ToString();
	}

	private void CheckActions(IUnitScript unit)
	{
		int moveCost = unit.GetCostActions()[(int)Enums.Actions.Move];
		int attackCost = unit.GetCostActions()[(int)Enums.Actions.Attack];
		int defenseCost = unit.GetCostActions()[(int)Enums.Actions.Defense];
		int abilityCost = unit.GetCostActions()[(int)Enums.Actions.Ability];

		int mana = gameController.GetMana();
		int manaBuffer = gameController.GetManaBuffer();

		moveButton.interactable = mana >= moveCost && (mana - manaBuffer) >= moveCost;
		attackButton.interactable = mana >= attackCost && (mana - manaBuffer) >= attackCost;
		defenseButton.interactable = mana >= defenseCost && (mana - manaBuffer) >= defenseCost;
		abilityButton.interactable = mana >= abilityCost && (mana - manaBuffer) >= abilityCost;
	}

	private void UpdateResources (IUnitScript unit){
		SetAlfa(attackImage, 1.0f);
		SetAlfa(defenseImage, 1.0f);
		SetAlfa(moveImage, 1.0f);
		SetAlfa(abilityImage, 1.0f);
		switch (unit.type)
		{
			case "tank":
				attackImage.sprite = attackSpriteTankUnit;
				abilityImage.sprite = null;// TODO : quan hi hagi icona es ficara
				SetAlfa(abilityImage, 0f);
				break;
			case "ranged":
				attackImage.sprite = unit.team == 0 ? attackSpriteRangeUnitBlue : attackSpriteRangeUnitRed;
				abilityImage.sprite = abilitySpriteRangeUnit;
				break;
			case "melee":
				attackImage.sprite = unit.team == 0 ? attackSpriteCombatBlue : attackSpriteCombatRed;
				abilityImage.sprite = null; // TODO : quan hi hagi icona es ficara
				SetAlfa(abilityImage, 0f);
				break;
			case "healer":
				attackImage.sprite = unit.team == 0 ? attackSpriteRangeUnitBlue : attackSpriteRangeUnitRed;
				abilityImage.sprite = unit.team == 0 ? abilitySpriteHealerBlue : abilitySpriteHealerRed;
				break;
			default:
				break;
		}
		defenseImage.sprite = unit.team == 0 ? defenseSpriteBlue : defenseSpriteRed;
	}

	private void Show() {
		gameObject.SetActive(true);
	}

	private void Hide(){
		gameObject.SetActive(false);
	}

	private void InitElements()
	{
		var actions = this.gameObject.transform.Find("Actions");
		var move = actions.Find("Move");
		var attack = actions.Find("Attack");
		var defense = actions.Find("Defense");
		var ability = actions.Find("Ability");
		#region Buttons
		/*Buttons*/
		moveButton = move.Find("Button").GetComponent<Button>();
		attackButton = attack.Find("Button").GetComponent<Button>();
		defenseButton = defense.Find("Button").GetComponent<Button>();
		abilityButton = ability.Find("Button").GetComponent<Button>();
		#endregion
		#region Images
		/*Images*/
		moveImage = move.Find("Image").GetComponent<Image>();
		attackImage = attack.Find("Image").GetComponent<Image>();
		defenseImage = defense.Find("Image").GetComponent<Image>();
		abilityImage = ability.Find("Image").GetComponent<Image>();
		#endregion
		#region Cost Texts
		/*Cost Texts*/
		moveCost = move.Find("Cost").GetComponent<Text>();
		attackCost = attack.Find("Cost").GetComponent<Text>();
		defenseCost = defense.Find("Cost").GetComponent<Text>();
		abilityCost = ability.Find("Cost").GetComponent<Text>();
		#endregion
		#region Button's Listeners
		/*Button Listeners*/
		moveButton.onClick.AddListener(() => MoveButton());
		attackButton.onClick.AddListener(() => AttackButton());
		defenseButton.onClick.AddListener(() => DeffenseButton());
		abilityButton.onClick.AddListener(() => AbilityButton());
		#endregion
	}

	private void LoadResources(){
		#region Panel Sprites
		/*Panel Sprites*/
		blueBackgroundSprite = Resources.Load<Sprite>("panel-background-blue");
		redBackgroundSprite = Resources.Load<Sprite>("panel-background-red");
		#endregion
		#region Range Unit Sprites
		/*Range Unit Sprites*/
		attackSpriteRangeUnitBlue = Resources.Load<Sprite>("attack_icon_ranged_blue");
		attackSpriteRangeUnitRed = Resources.Load<Sprite>("attack_icon_ranged_red");
		abilitySpriteRangeUnit = Resources.Load<Sprite>("grenade_icon");
		#endregion
		#region Tank Unit Sprites
		/*Tank Unit Sprites*/
		attackSpriteTankUnit = Resources.Load<Sprite>("attack_icon_tank");
		#endregion
		#region Combat Unit Sprites
		/*Combat Unit Sprites*/
		attackSpriteCombatBlue = Resources.Load<Sprite>("icon_sword_blue");
		attackSpriteCombatRed = Resources.Load<Sprite>("icon_sword_red");
		#endregion
		#region Healer Unit Sprites
		/*Healer*/
		abilitySpriteHealerBlue = Resources.Load<Sprite>("heal_icon_blue");
		abilitySpriteHealerRed = Resources.Load<Sprite>("heal_icon_red");
		#endregion
		#region Defense Sprites
		/*Defense Sprite*/
		defenseSpriteBlue = Resources.Load<Sprite>("shield_blue_solid");
		defenseSpriteRed = Resources.Load<Sprite>("shield_red_solid");
		#endregion
	}

	#region Listeners
	private void MoveButton()
	{
		gameController.Move();
		if (!gameController.IsCancelAction())//si la accio ha estat cancelada, desseleccionem el boto
		{
			UnselectButton();
		}
	}

	private void AttackButton()
	{
		gameController.Attack();
		if (!gameController.IsCancelAction())//si la accio ha estat cancelada, desseleccionem el boto
		{
			UnselectButton();
		}
	}

	private void DeffenseButton()
	{
		gameController.Deffense();
		if (!gameController.IsCancelAction())//si la accio ha estat cancelada, desseleccionem el boto
		{
			UnselectButton();
		}
	}

	private void AbilityButton()
	{
		gameController.SpecialHability();
		if (!gameController.IsCancelAction())//si la accio ha estat cancelada, desseleccionem el boto
		{
			UnselectButton();
		}
	}
#endregion



	private void SetAlfa(Image image, float alfa)
	{
		Color color = image.color;
		color.a = alfa;
		image.color = color;
	}

	private void SetTeamSprite(Sprite sprite)
	{
		this.unitPreview.GetComponent<Image>().sprite = sprite;
	}

	private void UnselectButton()
	{
		GameObject myEventSystem = GameObject.Find("EventSystem");
		myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
	}
}
