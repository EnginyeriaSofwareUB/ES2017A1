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
    private Text healthText;
    [SerializeField]
    private Text attackText;
    [SerializeField]
    private Text defenseText;

	[SerializeField]
	private IUnitScript unit;

	// Use this for initialization
	void Start()
    {
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
		this.healthText.text = "Health Points: " + unit.Life;
        this.attackText.text = "Attack: " + unit.GetAttack;
        this.defenseText.text = "Defense: " + unit.GetDefenseModifier;
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
