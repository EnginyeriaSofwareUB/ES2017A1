using System.Collections;
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

    private TimeController timeController;

    public List<IUnitScript> units;
    private GameController gameController;
    private List<Point> currentRange = new List<Point>();
    private int columns = 30;
    private int rows = 30;
    private int rangeToSpawn = 4;

    private bool isPowerUp = false;

    public float TileSize
    {
        get { return tile[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    // Use this for initialization
    public void Init()
    {
        GameObject infoSelection = GameObject.Find("InfoSelection");
        gameController = GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>();
        List<string> blueUnitsName = infoSelection.GetComponent<InfoSelection>().BlueUnits;
        List<GameObject> blueUnitsFromSelection = new List<GameObject>();
        GameObject tmp;
        foreach(string b in blueUnitsName)
        {
            tmp = Resources.Load("Units/"+b) as GameObject;
            blueUnitsFromSelection.Add(tmp);
        }
        blueUnits = blueUnitsFromSelection.ToArray();
        List<string> redUnitsName = infoSelection.GetComponent<InfoSelection>().RedUnits;
        List<GameObject> redUnitsFromSelection = new List<GameObject>();
        foreach(string r in redUnitsName)
        {
            tmp = Resources.Load("Units/" + r) as GameObject;
            redUnitsFromSelection.Add(tmp);
        }
        redUnits = redUnitsFromSelection.ToArray();
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
                PlaceTile(Converter(newTiles[x].ToString()), x, y, worldStart);
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
        return (Tiles.ContainsKey(point) && Tiles[point].GetIsEmpty());
    }

    private bool IsWithinBounds(Point point)
    {
        return Tiles.ContainsKey(point);
    }

    public void ShowRange(Point position, int range)
    {
        Point currentPoint, newPoint;
        List<Point> buffer = new List<Point>();
        buffer.Add(position);

        if (gameController.GetHability() == "Move" || gameController.ActualUnit.type == "tank")
        {
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
        }
        else if (gameController.GetHability() == "Attack")
        {
            while (buffer.Any())
            {
                currentPoint = buffer.First();
                newPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                if (!currentRange.Contains(newPoint) && Distance(position, newPoint) <= range)
                    buffer.Add(newPoint);

                newPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                if (!currentRange.Contains(newPoint) && Distance(position, newPoint) <= range)
                    buffer.Add(newPoint);

                newPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                if (!currentRange.Contains(newPoint) && Distance(position, newPoint) <= range)
                    buffer.Add(newPoint);

                newPoint = new Point(currentPoint.X, currentPoint.Y + 1);
                if (!currentRange.Contains(newPoint) && Distance(position, newPoint) <= range)
                    buffer.Add(newPoint);


                currentRange.Add(currentPoint);
                buffer.Remove(currentPoint);

            }
        }

        currentRange.Remove(position);
        foreach(var point in currentRange)
        {
            if(gameController.GetHability() == "Move")
            {
                Tiles[point].SetColor(Color.blue);
            }
            else if(gameController.GetHability() == "Special")
            {
                Tiles[point].SetColor(Color.yellow);
            }
            else if (gameController.GetHability() == "Attack" && gameController.ActualUnit.type == "tank")
            {
                Tiles[point].SetColor(Color.blue);
            }
        }
    }

    public void LineRange(Point starterPoint, int range)
    {
        Point currentPoint;
        currentPoint = starterPoint;
        do
        {
            currentPoint.X++;
            currentRange.Add(currentPoint);
        } while (IsValidTile(currentPoint) && Distance(starterPoint, currentPoint) < range);

        currentPoint = starterPoint;
        do
        {
            currentPoint.X--;
            currentRange.Add(currentPoint);
        } while (IsValidTile(currentPoint) && Distance(starterPoint, currentPoint) < range);

        currentPoint = starterPoint;
        do
        {
            currentPoint.Y--;
            currentRange.Add(currentPoint);
        } while (IsValidTile(currentPoint) && Distance(starterPoint, currentPoint) < range);

        currentPoint = starterPoint;
        do
        {
            currentPoint.Y++;
            currentRange.Add(currentPoint);
        } while (IsValidTile(currentPoint) && Distance(starterPoint, currentPoint) < range);

        foreach (var point in currentRange)
        {
            Tiles[point].SetColor(Color.yellow);
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
                gameController.MakeInteractableButtons(true);
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
                gameController.ActualUnit.SetSelected(false);
                gameController.ActualCell = null;
                gameController.ActualUnit = null;
                gameController.SetAbility(" ");
                gameController.SetCancelAction(false);
                ClearCurrentRange();
                gameController.HidePlayerStats();
                gameController.FinishAction();
            }
            else if (gameController.ActualUnit.GetType() == "ranged" && gameController.GetHability() == "Ability")
            {
                gameController.destinationPoint = point;
                gameController.ActualUnit.UseAbility();
                gameController.HidePlayerStats();
                gameController.ActualCell = null;
                gameController.ActualUnit = null;
                gameController.SetAbility(" ");
                gameController.SetCancelAction(false);
                gameController.HidePlayerStats();
                gameController.FinishAction();
            }
            else if (gameController.ActualUnit.GetType() == "melee" && gameController.GetHability() == "Special")
            {

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
            xRandom = Random.Range(rows - rangeToSpawn - 1, rows - 2);
            yRandom = Random.Range(columns - rangeToSpawn - 1, columns - 2);
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
        TextAsset bindData = Resources.Load("NewMap") as TextAsset;
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

    public Vector3 GetPosition(Point point)
    {
        return Tiles[point].WorldPosition;
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

    public void DamageInRangeForTank(Point point, int range, double damage)
    {
        foreach (var unit in units)
        {
            if (Distance(unit.currentPosition, point) < range + 1 && unit.team != gameController.ActualUnit.team)
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


    public void PaintSurrounding(Point point)
    {
        Tiles[new Point(point.X+1, point.Y)].SetColor(Color.yellow);
        Tiles[new Point(point.X, point.Y+1)].SetColor(Color.yellow);
        Tiles[new Point(point.X-1, point.Y)].SetColor(Color.yellow);
        Tiles[new Point(point.X, point.Y-1)].SetColor(Color.yellow);
    }

    public void ClearSurrounding(Point point, Color color)
    {
        Tiles[new Point(point.X + 1, point.Y)].SetColor(color);
        Tiles[new Point(point.X, point.Y + 1)].SetColor(color);
        Tiles[new Point(point.X - 1, point.Y)].SetColor(color);
        Tiles[new Point(point.X, point.Y - 1)].SetColor(color);
    }

    public void KillUnits(string color)
    {
        switch (color) {
            case "Blue":
                foreach (var player in GameObject.FindGameObjectsWithTag("Blue")) {
                    Destroy(player.transform.gameObject);
                }
                break;
            case "Red":
                foreach (var player in GameObject.FindGameObjectsWithTag("Red")) {
                    Destroy(player.transform.gameObject);
                }
                break;
            case "Default":
                break;
        }
    }

    public void TriggerMeteorit(int team)
    {
        var unitsFromTeam = units.Where(u => u.team == team);
        var unit = unitsFromTeam.ElementAt(Random.Range(0, unitsFromTeam.Count()));
        int distance = 2;
        var currentPoint = unit.currentPosition;
        while(distance > 0)
        {
            int rand = Random.Range(0, 4);
            switch(rand)
            {
                case 0:
                    currentPoint.X++;
                    if (IsWithinBounds(currentPoint)) distance--;
                    else currentPoint.X--;
                    break;
                case 1:
                    currentPoint.X--;
                    if (IsWithinBounds(currentPoint)) distance--;
                    else currentPoint.X++;
                    break;
                case 2:
                    currentPoint.Y++;
                    if (IsWithinBounds(currentPoint)) distance--;
                    else currentPoint.Y--;
                    break;
                case 3:
                    currentPoint.Y--;
                    if (IsWithinBounds(currentPoint)) distance--;
                    else currentPoint.Y++;
                    break;
            }
        }
        StartCoroutine(MeteorInPoint(currentPoint));
    }

    IEnumerator MeteorInPoint(Point position)
    {
        Point currentPoint, newPoint;
        List<Point> buffer = new List<Point>();
        int range = 2;
        buffer.Add(position);

        GameObject meteorit = Instantiate(Resources.Load("Objects/meteorite_flames_0")) as GameObject;
        meteorit.transform.position = new Vector3(Tiles[position].transform.position.x, Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y, 0);
        var meteorScript = meteorit.GetComponent<MeteorObjectScript>();
        meteorScript.targetPosition = Tiles[position].transform.position;
        meteorScript.started = true;
        yield return new WaitUntil(() => meteorScript.finished);
        while (buffer.Any())
        {
            currentPoint = buffer.First();
            newPoint = new Point(currentPoint.X + 1, currentPoint.Y);
            if (IsWithinBounds(newPoint) && !currentRange.Contains(newPoint) && Distance(position, newPoint) <= range)
                buffer.Add(newPoint);

            newPoint = new Point(currentPoint.X - 1, currentPoint.Y);
            if (IsWithinBounds(newPoint) && !currentRange.Contains(newPoint) && Distance(position, newPoint) <= range)
                buffer.Add(newPoint);

            newPoint = new Point(currentPoint.X, currentPoint.Y - 1);
            if (IsWithinBounds(newPoint) && !currentRange.Contains(newPoint) && Distance(position, newPoint) <= range)
                buffer.Add(newPoint);

            newPoint = new Point(currentPoint.X, currentPoint.Y + 1);
            if (IsWithinBounds(newPoint) && !currentRange.Contains(newPoint) && Distance(position, newPoint) <= range)
                buffer.Add(newPoint);

            currentRange.Add(currentPoint);
            buffer.Remove(currentPoint);
        }

        GameObject expl = Instantiate(Resources.Load("Objects/ExplosionRed")) as GameObject;
        expl.transform.localScale += new Vector3(5f, 5f, 0);
        expl.transform.position = meteorScript.targetPosition;
        Destroy(meteorit);
        Destroy(expl, 0.5f);

        var unitsToRemove = new List<IUnitScript>();

        foreach (var unit in units)
        {
            if (currentRange.Contains(unit.currentPosition))
            {
                Destroy(unit.transform.gameObject);
                unitsToRemove.Add(unit);
            }
        }

        units.RemoveAll(x => unitsToRemove.Contains(x));

        foreach (var point in currentRange)
        {
            if (!Tiles.ContainsKey(point)) continue;
            Destroy(Tiles[point].transform.gameObject);
            Tiles.Remove(point);
        }
        
        currentRange = new List<Point>();

    }

    IEnumerator Pause(MeteorObjectScript meteorScript)
    {
        yield return new WaitUntil(() => meteorScript.finished == true);
    }

    public string Converter(string s)
    {
        string c = "";
        switch (s)
        {
            case "A":
                c = "10";
                break;
            case "B":
                c = "11";
                break;
            case "C":
                c = "12";
                break;
            case "D":
                c = "13";
                break;
            case "E":
                c = "14";
                break;
            case "F":
                c = "15";
                break;
            case "G":
                c = "16";
                break;
            case "H":
                c = "17";
                break;
            case "I":
                c = "18";
                break;
            case "J":
                c = "19";
                break;
            case "K":
                c = "20";
                break;
            case "L":
                c = "21";
                break;
            case "M":
                c = "22";
                break;
            case "N":
                c = "23";
                break;
            case "O":
                c = "24";
                break;
            case "P":
                c = "25";
                break;
            case "Q":
                c = "26";
                break;
            case "R":
                c = "27";
                break;
            default:
                c = s;
                break;
        }
        return c;
    }

    public void SpawnPowerUp()
    {
        int xRandom = Random.Range(2, columns-1);
        int yRandom = Random.Range(2, rows-1);
        Point position = new Point(xRandom, yRandom);
        while (!Tiles[position].GetIsEmpty())
        {
            xRandom = Random.Range(2, columns-1);
            yRandom = Random.Range(2, rows-1);
            position = new Point(xRandom, yRandom);
        }
        GameObject powerUp = Instantiate(Resources.Load("Objects/PowerUp")) as GameObject;
        powerUp.GetComponent<PowerUpScript>().Setup(position, Tiles[position].transform.position, map);
        this.isPowerUp = true;
        //Tiles[position].SetIsEmpty(false);
    }

    public void GetPowerUp(int team)
    {
        this.TriggerMeteorit(team);
        GameObject powerUp = GameObject.FindGameObjectWithTag("PowerUp");
        Destroy(powerUp);
        this.isPowerUp = false;
        timeController = GameObject.FindObjectOfType<TimeController>();
        timeController.TimeForPowerUp(Random.Range(120, 300));
        timeController.SetCountDownPowerUpActivate(true);
    }

    public bool IsPowerUpActivate()
    {
        return this.isPowerUp;
    }
}
