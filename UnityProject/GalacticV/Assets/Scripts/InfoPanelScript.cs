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
    private Image unitPreview;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text attackText;
    [SerializeField]
    private Text defenseText;

    // Use this for initialization
    void Start()
    {
		Hide ();
    }

    public void ShowPanel(UnitScript unit)
    {
        PrintStats(unit);
		SetTeamImage(unit.gameObject.GetComponent<SpriteRenderer>());
		Show ();
    }


    public void PrintStats(UnitScript unit)
    {
        this.healthText.text = "Health Points: " + unit.GetHealthPoints();
        this.attackText.text = "Attack: " + unit.GetAttackDamage();
        this.defenseText.text = "Defense: " + unit.GetDefensePoints();
    }

    public void SetTeamImage(SpriteRenderer spriteRenderer)
    {
		this.unitPreview.sprite = spriteRenderer.sprite;
		/*Animator anim = this.unitPreview.GetComponent<Animator>();
		anim.runtimeAnimatorController = animator.runtimeAnimatorController;
		anim.updateMode = AnimatorUpdateMode.UnscaledTime;*/

    }

	public void HidePanel(){
		Hide ();
	}

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide(){
		gameObject.SetActive(false);
	}
}
