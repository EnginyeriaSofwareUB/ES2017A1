using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelScript : MonoBehaviour
{
    private GameController gameController;

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
        gameController = GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>();
        gameObject.SetActive(false);
    }

    public void ShowPanel(UnitScript unit)
    {
        PrintStats(unit);
        SetTeamImage(unit.GetSpriteRenderer());
        gameObject.SetActive(true);
    }


    public void PrintStats(UnitScript unit)
    {
        this.healthText.text = "Health Points: " + unit.GetHealthPoints();
        this.attackText.text = "Attack: " + unit.GetAttackDamage();
        this.defenseText.text = "Defense: " + unit.GetDefensePoints();
    }

    public void SetTeamImage(SpriteRenderer spriteRendered)
    {
        SpriteRenderer sprite = this.unitPreview.GetComponent<SpriteRenderer>();
        sprite.sprite = spriteRendered.sprite;
    }
}
