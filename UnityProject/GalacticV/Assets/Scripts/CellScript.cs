using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour {

    public Point GridPosition { get; private set; }

    private Color32 emptyColor = new Color32(96, 255, 90, 255);
    private SpriteRenderer spriteRenderer;
    private bool isEmpty;

    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isEmpty = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
    }

    private void OnMouseOver()
    {
        if (tag != "Border")
        {
            spriteRenderer.color = emptyColor;
        }
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
    }

    public bool GetIsEmpty()
    {
        return this.isEmpty;
    }

    public void SetIsEmpty(bool _isEmpty)
    {
        this.isEmpty = _isEmpty;
    }
}
