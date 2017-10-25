﻿using System;
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

    // Use this for initialization
    void Start()
    {
		Hide ();
    }

    public void ShowPanel(IUnitScript unit)
    {
        PrintStats(unit);
		SetTeamImage(unit.gameObject.GetComponent<SpriteRenderer>());
		//SetAnimator(unit.gameObject.GetComponent<Animator>());
		Show ();
    }


    public void PrintStats(IUnitScript unit)
    {
		this.healthText.text = "Health Points: " + unit.GetLifeValue();
        this.attackText.text = "Attack: " + unit.GetAttackValue();
        this.defenseText.text = "Defense: " + unit.GetDefenseModifier();
    }

    public void SetTeamImage(SpriteRenderer spriteRenderer)
    {
		this.unitPreview.GetComponent<Image>().sprite = spriteRenderer.sprite;
    }

	public void SetAnimator(Animator animator)
	{
		Animator anim = this.unitPreview.GetComponent<Animator>();
		//anim = animator;
		anim.runtimeAnimatorController = animator.runtimeAnimatorController;
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
