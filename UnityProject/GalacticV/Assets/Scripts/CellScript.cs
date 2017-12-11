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
    private GameController gameController;

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
        gameController = GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>();
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
            switch (gameController.GetHability())
            {
                case "Attack":
                    previousColor = (spriteRenderer.color != emptyColor) ? spriteRenderer.color : previousColor;
                    spriteRenderer.color = emptyColor;
                    break;
                case "Ability":
					if (gameController.ActualUnit.type == "healer")
					{
						spriteRenderer.color = Color.yellow;
					}
					else
					{
						//previousColor = (spriteRenderer.color != emptyColor) ? spriteRenderer.color : previousColor
						spriteRenderer.color = Color.yellow;
						MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
						manager.PaintSurrounding(this.GridPosition);
					}
                    break;
                default:
                    previousColor = (spriteRenderer.color != emptyColor) ? spriteRenderer.color : previousColor;
                    spriteRenderer.color = emptyColor;
                    break;
            }
        }
    }

    private void OnMouseExit()
    {
        gameController = GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>();
        if (tag != "Border" && gameController.ActualCell != this)
        {
            spriteRenderer.color = previousColor;
        }
        if (gameController.GetHability() == "Ability")
        {
            MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
            manager.ClearSurrounding(this.GridPosition, previousColor);
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
        Debug.Log(GridPosition.X + " " + GridPosition.Y);
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        if (gameController.GetHability() == "Ability")
        {
            spriteRenderer.color = previousColor;
            manager.ClearSurrounding(this.GridPosition, previousColor);
        }
        if (isEmpty)
            manager.RecievedClickOnCell(GridPosition);
        

    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void PaintSelected()
    {
        spriteRenderer.color = emptyColor;
    }

    public void PaintUnselected()
    {
        spriteRenderer.color = Color.white;
    }
}
