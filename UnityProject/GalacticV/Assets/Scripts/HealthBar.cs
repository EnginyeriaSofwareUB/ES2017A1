using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Vector2 initSize;
    private GameObject unit;
    private GameObject content;
    private Color half = Color.yellow;
    private Color low = Color.red;
    private float maxLife = 8;

    void Start()
    {
        unit = gameObject.transform.parent.transform.parent.gameObject;
        maxLife = unit.GetComponent<IUnitScript>().Life;
        content = transform.GetChild(0).gameObject;
        initSize = content.GetComponent<RectTransform>().sizeDelta;
    }

    public void ReduceLife(float actualLife)
    {
        RectTransform r = content.GetComponent<RectTransform>();
        float actualWidht = (initSize.x * actualLife) / maxLife;
        r.sizeDelta = new Vector2(actualWidht, initSize.y);
        float percent = (actualLife / maxLife) * 100f;
        if(percent <= 50f && percent > 25f)
        {
            content.GetComponent<Image>().color = half;
        }
        else if(percent <= 25f)
        {
            content.GetComponent<Image>().color = low;
        }
    }

}
