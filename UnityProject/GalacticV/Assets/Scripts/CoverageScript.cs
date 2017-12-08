using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverageScript : MonoBehaviour
{

    [SerializeField]
    private bool full;
    [SerializeField]
    private Sprite halfCoverage;

    public Point GridPosition { get; private set; }


    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
    }

    public void ChangeSprite()
    {
        //gameObject.GetComponent<SpriteRenderer>().sprite = halfCoverage;
        full = false;
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        gameObject.AddComponent<BoxCollider2D>();
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        gameObject.GetComponent<Animator>().SetTrigger("half");
        /*
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.874403f, 0.7641022f);
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.0058017f, 0.5912942f);*/
    }

    public bool IsFull()
    {
        return full;
    }
}
