using System.Collections.Generic;
using System.Linq;
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
        SpawnCoverage();
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
        Point currentPoint, newPoint;
        List<Point> buffer = new List<Point>();
        buffer.Add(position);
        while (buffer.Any())
        {
            currentPoint = buffer.First();
            newPoint = new Point(currentPoint.X + 1, currentPoint.Y);
            if (IsValidTile(newPoint) && !currentRange.Contains(newPoint) && Distance(position, newPoint) <= range)
                buffer.Add(newPoint);

            newPoint = new Point(currentPoint.X - 1, currentPoint.Y);
            if (IsValidTile(newPoint) && !currentRange.Contains(newPoint) && Distance(position, newPoint) <= range)
                buffer.Add(newPoint);

            newPoint = new Point(currentPoint.X, currentPoint.Y - 1);
            if (IsValidTile(newPoint) && !currentRange.Contains(newPoint) && Distance(position, newPoint) <= range)
                buffer.Add(newPoint);

            newPoint = new Point(currentPoint.X, currentPoint.Y + 1);
            if (IsValidTile(newPoint) && !currentRange.Contains(newPoint) && Distance(position, newPoint) <= range)
                buffer.Add(newPoint);


            currentRange.Add(currentPoint);
            buffer.Remove(currentPoint);

        }

        currentRange.Remove(position);
        foreach(var point in currentRange)
        {
            if(gameController.GetHability() == "Move")
            {
                Tiles[point].SetColor(Color.cyan);
            }
            else if(gameController.GetHability() == "Special")
            {
                Tiles[point].SetColor(Color.yellow);
            }
        }
    }

    private bool CheckConnection(Point currentPoint, List<Point> currentRange)
    {
        return currentRange.Any(x => Distance(x, currentPoint) == 1);
    }

    private int Distance(Point p1, Point p2)
    {
        //Manhattan distance
        //Note: System is called manually to not create issues with Random() calls
        return System.Math.Abs(p1.X - p2.X) + System.Math.Abs(p1.Y - p2.Y);
    }

    public void ClearCurrentRange()
    {
        foreach (var point in currentRange)
        {
            Tiles[point].SetColor(Color.white);
        }
        currentRange = new List<Point>();
        gameController.SetAbility(" ");
    }

    public void RecievedClickOnCell(Point point)
    {
        if (gameController.ActualUnit != null)
        {
            if (currentRange.Contains(point) && gameController.GetHability() == "Move")
            {
                gameController.ActualCell.PaintUnselected();
                Tiles[gameController.ActualUnit.currentPosition].SetIsEmpty(true);
                List<Vector3> movementPath = CalculatePath(gameController.ActualUnit.currentPosition, point);
                gameController.ActualUnit.MoveTo(point, movementPath);
                Tiles[point].SetIsEmpty(false);
                gameController.ActualCell = null;
                gameController.ActualUnit = null;
                ClearCurrentRange();
				gameController.HidePlayerStats();
            }
            else if(gameController.ActualUnit.GetType() == "tank" && currentRange.Contains(point) && gameController.GetHability() == "Special")
            {
                gameController.ActualCell.PaintUnselected();
                Tiles[gameController.ActualUnit.currentPosition].SetIsEmpty(true);
                string coverageName = "";
                if(gameController.ActualUnit.team == 0)
                {
                    coverageName = "Objects/CoverageBlue";
                }
                else if(gameController.ActualUnit.team == 1)
                {
                    coverageName = "Objects/CoverageRed";
                }
                GameObject coverage = Instantiate(Resources.Load(coverageName)) as GameObject;
                coverage.GetComponent<CoverageScript>().Setup(point, Tiles[point].transform.position, map);
                coverage.transform.Rotate(0, 0, 45);
                Tiles[point].SetIsEmpty(false);
                gameController.ActualCell = null;
                gameController.ActualUnit = null;
                gameController.SetAbility(" ");
                gameController.SetCancelAction(false);
                ClearCurrentRange();
                gameController.HidePlayerStats();
                gameController.FinishAction();
            }
        }
    }

    //Pathfinding using DFS
    private List<Vector3> CalculatePath(Point start, Point end)
    { 
        List<Vector3> path;
        List<Point> visited = new List<Point>();
        Queue<Point> stack = new Queue<Point>();
        Dictionary<Point, Point> parents = new Dictionary<Point, Point>();
        stack.Enqueue(start);
        Point currentPoint, newPoint;
        while(stack.Any())
        {
            currentPoint = stack.Dequeue();
            if (currentPoint.Equals(end)) break;
            if (!visited.Contains(currentPoint))
            {
                visited.Add(currentPoint);
                newPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                if (currentRange.Contains(newPoint))
                {
                    stack.Enqueue(newPoint);
                    if (!parents.ContainsKey(newPoint))
                        parents.Add(newPoint, currentPoint);
                }

                newPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                if (currentRange.Contains(newPoint))
                {
                    stack.Enqueue(newPoint);
                    if (!parents.ContainsKey(newPoint))
                        parents.Add(newPoint, currentPoint);
                }

                newPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                if (currentRange.Contains(newPoint))
                {
                    stack.Enqueue(newPoint);
                    if (!parents.ContainsKey(newPoint))
                        parents.Add(newPoint, currentPoint);
                }

                newPoint = new Point(currentPoint.X, currentPoint.Y + 1);
                if (currentRange.Contains(newPoint))
                {
                    stack.Enqueue(newPoint);
                    if (!parents.ContainsKey(newPoint))
                        parents.Add(newPoint, currentPoint);
                }
            }
        }
        visited = new List<Point>();
        currentPoint = end;
        while(!currentPoint.Equals(start))
        {
            visited.Add(currentPoint);
            currentPoint = parents[currentPoint];
        }

        return visited.Select(point => Tiles[point].transform.position).Reverse().ToList();
    }

    public void SpawnUnits()
    {
        for(int i = 0; i < blueUnits.Length; ++i)
        {
            SpawnBlueUnits(i);
            SpawnRedUnits(i);
        }
    }

    public void SpawnRedUnits(int i)
    {
        int xRandom = Random.Range(rows-rangeToSpawn-1, rows-2);
        int yRandom = Random.Range(columns-rangeToSpawn-1, columns-2);
        IUnitScript newUnit = Instantiate(redUnits[i]).GetComponent<IUnitScript>();
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

    public void SpawnBlueUnits(int i)
    {
        int yRandom = Random.Range(1, rangeToSpawn+1);
        int xRandom = Random.Range(1, rangeToSpawn+1);
        IUnitScript newUnit = Instantiate(blueUnits[i]).GetComponent<IUnitScript>();
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
        TextAsset bindData = Resources.Load("the_palace") as TextAsset;
        string data = bindData.text.Replace(System.Environment.NewLine, string.Empty);
        return data.Split('-');
    }

    public void SpawnCoverage()
    {
        for(int i = 0; i < 2; ++i)
        {
            int xRandom = Random.Range(3, rows/2);
            int yRandom = (rows - 2) - xRandom;
            Point position = new Point(xRandom, yRandom);
            while (!Tiles[position].GetIsEmpty())
            {
                xRandom = Random.Range(1, (rows - 2) / 2);
                yRandom = (rows - 2) - xRandom;
                position = new Point(xRandom, yRandom);
            }
            GameObject coverage = Instantiate(Resources.Load("Objects/Tree")) as GameObject;
            coverage.GetComponent<CoverageScript>().Setup(position, Tiles[position].transform.position, map);
            Tiles[position].SetIsEmpty(false);
        }   
    }

    public void DamageInRange(Point point, int range, double damage)
    {
        foreach (var unit in units)
        {
            if (Distance(unit.currentPosition, point) < range + 1)
            {
                if (damage >= unit.Life)
                {
                    Tiles[unit.currentPosition].SetIsEmpty(true);
                    Tiles[unit.currentPosition].SetColor(Color.white);
                    Destroy(unit.transform.gameObject);
                }
                else
                {
                    unit.Life -= (float)damage;
                    unit.ReduceLife();
                    Tiles[unit.currentPosition].SetColor(Color.white);
                }
            }
        }

        
    }
}
