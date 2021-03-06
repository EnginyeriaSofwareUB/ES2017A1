﻿using System.Collections;
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        MapManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();

        if (collision.tag == "Red") {
            manager.GetPowerUp(0);
        } else if (collision.tag == "Blue")
        {
            manager.GetPowerUp(1);
        }

    }
}
