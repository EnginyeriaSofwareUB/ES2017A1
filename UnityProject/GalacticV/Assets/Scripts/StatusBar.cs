using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{

	[SerializeField]
	private Text pointsText;

	private GameObject currentBar;
	private float ratio;
	private float initialWidth;

	private void Start()
	{
		currentBar = this.gameObject.transform.Find("image").gameObject;
		initialWidth = (this.currentBar.transform as RectTransform).sizeDelta.x;
	}

	public void UpdateStatusBar(float currentPoints, float maxPoints)
	{
		RectTransform r = currentBar.transform as RectTransform;
		float actualWidth = (initialWidth * currentPoints) / maxPoints;
		r.sizeDelta = new Vector2(actualWidth, r.sizeDelta.y);
		pointsText.text = currentPoints.ToString();
	}
}
