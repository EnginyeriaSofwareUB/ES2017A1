using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] tile;

    [SerializeField]
    private Transform map;

    public Dictionary<Point, CellScript> Tiles { get; set; }

    private int columns = 30;
    private int rows = 30;

    public float TileSize
    {
        get { return tile[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    // Use this for initialization
    void Start()
    {
        CreateLevel();
    }

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, CellScript>();

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (y == 0 || x == 0 || y == rows - 1 || x == columns - 1)
                {
                    PlaceTile("0", x, y, worldStart);
                }
                else
                {
                    PlaceTile("1", x, y, worldStart);
                }
            }
        }
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);

        CellScript newTile = Instantiate(tile[tileIndex]).GetComponent<CellScript>();
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map);
        Tiles.Add(new Point(x, y), newTile);
    }
}
