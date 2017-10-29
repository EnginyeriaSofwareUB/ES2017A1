using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{

	[SerializeField]
	private Text pointsText;
	[SerializeField]
	private GameObject currentBar;
	private float initialWidth;

	void Start()
	{
		initialWidth = (currentBar.transform as RectTransform).sizeDelta.x;
	}

	private void Awake()
	{
		initialWidth = (currentBar.transform as RectTransform).sizeDelta.x;
	}

	public void UpdateStatusBar(float currentPoints, float maxPoints)
	{
		RectTransform r = currentBar.transform as RectTransform;
		float actualWidth = (initialWidth * currentPoints) / maxPoints;
		r.sizeDelta = new Vector2(actualWidth, r.sizeDelta.y);
		pointsText.text = currentPoints.ToString();
	}
}
