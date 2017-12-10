using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {

    public Point GridPosition { get; private set; }

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hay colision");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hay trigger");
    }
}
