using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellScript : MonoBehaviour {

    public Point GridPosition { get; private set; }

    private Color32 emptyColor = new Color32(96, 255, 90, 255);
    private Color previousColor = Color.white;
    private SpriteRenderer spriteRenderer;
    private bool isEmpty = true;

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
            previousColor = (spriteRenderer.color != emptyColor) ? spriteRenderer.color : previousColor;
            spriteRenderer.color = emptyColor;
        }
    }

    private void OnMouseExit()
    {
        if (tag != "Border")
        {
            spriteRenderer.color = previousColor;
        }
    }

    public bool GetIsEmpty()
    {
        return this.isEmpty;
    }

    public void SetIsEmpty(bool _isEmpty)
    {
        this.isEmpty = _isEmpty;
    }

    private void OnMouseDown()
    {
        //Debug.Log(GridPosition.X + " " + GridPosition.Y);
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        if (isEmpty)
            manager.RecievedClickOnCell(GridPosition);

    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

}
