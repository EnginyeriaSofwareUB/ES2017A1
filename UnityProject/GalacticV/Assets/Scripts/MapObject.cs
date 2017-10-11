using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    private Color32 fullColor = new Color32(255, 118, 118, 255);
    private SpriteRenderer spriteRenderer;
    public Point pointGrid;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        spriteRenderer.color = fullColor;
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
    }

    public void SetPoint(int x, int y)
    {
        pointGrid = new Point(x, y);
    }
}
