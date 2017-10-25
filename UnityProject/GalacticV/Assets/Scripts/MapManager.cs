using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] tile;

    [SerializeField]
    private Transform map;

    [SerializeField]
    private GameObject[] blueUnits;

    [SerializeField]
    private GameObject[] redUnits;

    [SerializeField]
    public Dictionary<Point, CellScript> Tiles { get; set; }

    public List<IUnitScript> units;
    private GameController gameController;
    private List<Point> currentRange = new List<Point>();
    private int columns = 30;
    private int rows = 30;
    private int rangeToSpawn = 3;

    public float TileSize
    {
        get { return tile[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    // Use this for initialization
    public void Init()
    {
        gameController = GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>();
        CreateLevel();
        SpawnUnits();
        GameObject map = GameObject.Find("Map");
        map.transform.Rotate(0, 0, 45f);
    }

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, CellScript>();
        string[] mapData = ReadLevelText();
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3((Screen.width ) / 3, (Screen.height*3)/4));
        columns = mapData.Length;
        rows = mapData[0].ToCharArray().Length;
        for (int y = 0; y < columns; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < rows; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }

        units = new List<IUnitScript>();
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);

        CellScript newTile = Instantiate(tile[tileIndex]).GetComponent<CellScript>();
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map);
        Tiles.Add(new Point(x, y), newTile);
    }

    private bool IsValidTile(Point point)
    {
        return (point.X > 0 && point.X < columns - 1 && point.Y > 0 && point.Y < rows - 1 && Tiles[point].GetIsEmpty());
    }

    public void ShowRange(Point position, int range)
    {
        Point currentPoint;
        for (int i = 0; i <= range; ++i)
        {
            for (int j = 0; j <= range; j++)
            {
                if (i + j > range) continue;
                currentPoint = new Point(position.X + i, position.Y + j);
                if (IsValidTile(currentPoint))
                {
                    currentRange.Add(currentPoint);
                }
                currentPoint = new Point(position.X - i, position.Y - j);
                if (IsValidTile(currentPoint))
                {
                    currentRange.Add(currentPoint);
                }
                currentPoint = new Point(position.X + i, position.Y - j);
                if (IsValidTile(currentPoint))
                {
                    currentRange.Add(currentPoint);
                }
                currentPoint = new Point(position.X - i, position.Y + j);
                if (IsValidTile(currentPoint))
                {
                    currentRange.Add(currentPoint);
                }
            }
        }

        foreach(var point in currentRange)
        {
            Tiles[point].SetColor(Color.cyan);
        }
    }

    public void ClearCurrentRange()
    {
        foreach (var point in currentRange)
        {
            Tiles[point].SetColor(Color.white);
        }
        currentRange = new List<Point>();
        gameController.SetAbility("");
    }

    public void RecievedClickOnCell(Point point)
    {
        if (gameController.ActualUnit != null)
        {
            if (currentRange.Contains(point))
            {
                gameController.ActualCell.PaintUnselected();
                Tiles[gameController.ActualUnit.currentPosition].SetIsEmpty(true);
                gameController.ActualUnit.MoveTo(point, Tiles[point].transform.position);
                Tiles[point].SetIsEmpty(false);
                gameController.ActualCell = null;
                gameController.ActualUnit = null;
                ClearCurrentRange();
				gameController.HidePlayerStats();
            }
        }
    }

    public void SpawnUnits()
    {
        for(int i = 0; i < blueUnits.Length; ++i)
        {
            SpawnBlueUnits();
            SpawnRedUnits();
        }
    }

    public void SpawnRedUnits()
    {
        int xRandom = Random.Range(rows-rangeToSpawn-1, rows-2);
        int yRandom = Random.Range(columns-rangeToSpawn-1, columns-2);
        IUnitScript newUnit = Instantiate(redUnits[0]).GetComponent<IUnitScript>();
        Point position = new Point(xRandom, yRandom);
        while (!Tiles[position].GetIsEmpty())
        {
            yRandom = Random.Range(rows - rangeToSpawn - 1, rows - 2);
            xRandom = Random.Range(columns - rangeToSpawn - 1, columns - 2);
            position = new Point(xRandom, yRandom);
        }
        newUnit.Setup(position, Tiles[position].transform.position, map);
        units.Add(newUnit);
        Tiles[position].SetIsEmpty(false);
    }

    public void SpawnBlueUnits()
    {
        int yRandom = Random.Range(1, rangeToSpawn+1);
        int xRandom = Random.Range(1, rangeToSpawn+1);
        IUnitScript newUnit = Instantiate(blueUnits[0]).GetComponent<IUnitScript>();
        Point position = new Point(xRandom, yRandom);
        while(!Tiles[position].GetIsEmpty())
        {
            yRandom = Random.Range(1, rangeToSpawn+1);
            xRandom = Random.Range(1, rangeToSpawn+1);
            position = new Point(xRandom, yRandom);
        }
        newUnit.Setup(position, Tiles[position].transform.position, map);
        units.Add(newUnit);
        Tiles[position].SetIsEmpty(false);
    }

    public string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("LevelTest") as TextAsset;
        string data = bindData.text.Replace(System.Environment.NewLine, string.Empty);
        return data.Split('-');
    }
}
