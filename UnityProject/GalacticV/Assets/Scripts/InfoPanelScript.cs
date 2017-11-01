using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelScript : MonoBehaviour
{

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

	private Sprite blueBackgroundSprite;
	private Sprite redBackgroundSprite;

	[SerializeField]
	private IUnitScript unit;

	// Use this for initialization
	void Start()
    {
		blueBackgroundSprite = Resources.Load<Sprite>("panel-background-blue");
		redBackgroundSprite = Resources.Load<Sprite>("panel-background-red");
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
		}
	}

	public void ShowPanel(IUnitScript unit)
    {
		this.unit = unit;
		this.gameObject.GetComponent<Image>().sprite = unit.team == 0 ? blueBackgroundSprite : redBackgroundSprite;
		PrintStats(unit);
		Show();
	}

	public void HidePanel()
	{
		Hide();
		this.unit = null;
	}


	private void PrintStats(IUnitScript unit)
    {
		this.healthBar.GetComponent<StatusBar>().UpdateStatusBar((float) unit.Life, 8);
		this.attackBar.GetComponent<StatusBar>().UpdateStatusBar(unit.GetAttack, unit.GetAttack);
		this.defenseBar.GetComponent<StatusBar>().UpdateStatusBar((float) unit.GetDefenseModifier, (float)unit.GetDefenseModifier);
	}

	private void SetTeamSprite(Sprite sprite)
    {
		this.unitPreview.GetComponent<Image>().sprite = sprite;
    }

	private void Show() {
		gameObject.SetActive(true);
	}

	private void Hide(){
		gameObject.SetActive(false);
	}
}
