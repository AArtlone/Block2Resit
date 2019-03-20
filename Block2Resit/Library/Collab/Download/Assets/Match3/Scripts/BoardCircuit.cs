using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    wait,
    move
}

public enum TileKind
{
    Vertical,
    Horizontal,
    URCorner,
    LDCorner,
    LUCorner,
    DRCorner,
    Blank,
    Normal
}

[System.Serializable]
public class TileType
{
    public int x;
    public int y;
    public TileKind tileKind;
}

public class BoardCircuit : MonoBehaviour {

    public GameState currentState = GameState.move;
    private bool _gameStarted = false;

    public int width;
    public int height;
    public int offSet;
    public GameObject tilePrefab;
    public GameObject VerticalPrefab;
    public GameObject HorizontalPrefab;
    public GameObject URCornerPrefab;
    public GameObject LDCornerPrefab;
    public GameObject LUCornerPrefab;
    public GameObject DRCornerPrefab;

    private BackgroundTile[,] allTiles;
    private GameObject[,] circuitTilesGO;
    public GameObject[,] allDots;
    public GameObject[] dots;
    public TileType[] boardLayOut1;
    public TileType[] boardLayOut2;
    public TileType[] boardLayOut3;
    private FindMatchesCircuit findMatches;
    private RaycastShooter raycast;

    private int specialGemIterations = 0;
    public static int circuitsLightUp = 0;
    public static int circuitTilesCreated = 0;

    public GameObject _canvas;
    public GameObject circuitCanvas; //canvas that display circuit tiles info
    public Text currentText;//currrent amount of circuit tiles
    public Text neededText;//amount of circuit tiles needed
    public GameObject tick;
    public GameObject cross;

    // Use this for initialization
    void Start () {
        allTiles = new BackgroundTile[width, height];
        circuitTilesGO = new GameObject[width, height];
        allDots = new GameObject[width, height];
        findMatches = FindObjectOfType<FindMatchesCircuit>();
        raycast = FindObjectOfType<RaycastShooter>();
        //SetUpCircuit();
	}

    public void UpdateCircuitCanvas()
    {
        currentText.text = circuitsLightUp.ToString("0");
    }

    private IEnumerator CircuitCanvasCo()
    {
        yield return new WaitForSeconds(1f);
        circuitCanvas.gameObject.SetActive(true);
        currentText.text = circuitsLightUp.ToString("0");
        neededText.text = circuitTilesCreated.ToString("0");
    }

    public IEnumerator GameOverCo()
    {
        yield return new WaitForSeconds(1f);
        _canvas.SetActive(false);
        circuitCanvas.SetActive(false);
        //play sound
    }

    public void SetUpCircuit()
    {
        GenerateCircuitTiles();
        if (!_gameStarted)
        {
            _gameStarted = true;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector3 tempPosition = new Vector3(i * .3f, (j * .3f) + offSet, 0);

                    GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity);
                    backgroundTile.transform.parent = this.transform;
                    backgroundTile.transform.localPosition = tempPosition;
                    backgroundTile.name = "( " + i + ", " + j + " )";

                    int dotToUse = Random.Range(0, dots.Length);
                    int maxIterations = 0;
                    while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                    {
                        dotToUse = Random.Range(0, dots.Length);
                        maxIterations++;
                    }
                    maxIterations = 0;

                    GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    dot.GetComponent<Dot>().targetX = i;
                    dot.GetComponent<Dot>().targetY = j;
                    dot.transform.parent = this.transform;
                    dot.transform.localPosition = tempPosition;
                    dot.name = "( " + i + ", " + j + " )";
                    allDots[i, j] = dot;
                }
                StartCoroutine(CircuitCanvasCo());
            }
        }
    }

    public void ShuffleBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Destroy(allDots[i, j]);
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 tempPosition = new Vector3(i * .3f, (j * .3f) + offSet, 0);

                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity);
                backgroundTile.transform.parent = this.transform;
                backgroundTile.transform.localPosition = tempPosition;
                backgroundTile.name = "( " + i + ", " + j + " )";

                int dotToUse = Random.Range(0, dots.Length);
                int maxIterations = 0;
                while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, dots.Length);
                    maxIterations++;
                }
                maxIterations = 0;

                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<Dot>().targetX = i;
                dot.GetComponent<Dot>().targetY = j;
                dot.transform.parent = this.transform;
                dot.transform.localPosition = tempPosition;
                dot.name = "( " + i + ", " + j + " )";
                allDots[i, j] = dot;
            }
        }
    }

    //checks for matches while generating board
    private bool MatchesAt(int column, int row, GameObject piece){
        if(column > 1 && row > 1){
			if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
			{
				if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
				{
					return true;
				}
			}
			if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
			{
				if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
				{
					return true;
				}
			}

        } else if(column <= 1 || row <= 1){
            if(row > 1){
				if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
				{
					if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
					{
						return true;
					}
				}
            }
            if (column > 1)
            {
				if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
				{
					if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
					{
						return true;
					}
				}
            }
        }

        return false;
    }

    //actually destroys the Dot that is isMatched=true
    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
            findMatches.currentMatches.Remove(allDots[column, row]);
            //does a tile need to break?
            if (circuitTilesGO[column, row] != null)
            {
                //if it does give 1 damage
                circuitTilesGO[column, row].GetComponent<BackgroundTile>().LightUp();
                if (circuitTilesGO[column, row].GetComponent<BackgroundTile>().hitPoints <= 0)
                {
                    circuitTilesGO[column, row] = null;
                    circuitsLightUp++;
                    UpdateCircuitCanvas();
                    if(circuitsLightUp == circuitTilesCreated)
                    {
                        raycast.currentState = _GameState.gameOff;
                        currentState = GameState.wait;
                        StartCoroutine(GameOverCo());
                        RaycastShooter.circuitFinished = true;
                        cross.SetActive(false);
                        tick.SetActive(true);
                        RaycastShooter.puzzlesFinished++;
                        raycast.UpdateUI();
                    }
                }
            }
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }

    //cycles through all Dots and runs the DestroyMatchesAt function
    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    //decreases the row for each dot after matches were destroyed
    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector3 tempPosition = new Vector3(i * .3f, (j * .3f) + offSet, 0);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    dot.GetComponent<Dot>().targetX = i;
                    dot.GetComponent<Dot>().targetY = j;
                    dot.transform.parent = this.transform;
                    dot.transform.localPosition = tempPosition;
                    dot.name = "( " + i + ", " + j + " )";
                    allDots[i, j] = dot;
                }
            }
        }
    }


    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                   if(allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);
        while(MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        yield return new WaitForSeconds(1f);
        currentState = GameState.move;
    }

    public void GenerateCircuitTiles()
    {
        int presetToUse = Random.Range(1, 4);
        if(presetToUse == 1)
        {
            //look at all the tiles in the layout
            for (int i = 0; i < boardLayOut1.Length; i++)
            {
                //if a tile is a "Circuit" tile
                if (boardLayOut1[i].tileKind == TileKind.Vertical)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut1[i].x;
                    int itsY = boardLayOut1[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut1[i].x * .3f, boardLayOut1[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(VerticalPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;
                    
                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                } else if (boardLayOut1[i].tileKind == TileKind.Horizontal)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut1[i].x;
                    int itsY = boardLayOut1[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut1[i].x * .3f, boardLayOut1[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(HorizontalPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;
                    
                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                } else if (boardLayOut1[i].tileKind == TileKind.URCorner)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut1[i].x;
                    int itsY = boardLayOut1[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut1[i].x * .3f, boardLayOut1[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(URCornerPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;
                    
                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                } else if (boardLayOut1[i].tileKind == TileKind.LDCorner)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut1[i].x;
                    int itsY = boardLayOut1[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut1[i].x * .3f, boardLayOut1[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(LDCornerPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;
                    
                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                } else if (boardLayOut1[i].tileKind == TileKind.LUCorner)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut1[i].x;
                    int itsY = boardLayOut1[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut1[i].x * .3f, boardLayOut1[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(LUCornerPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;
                    
                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                } else if (boardLayOut1[i].tileKind == TileKind.DRCorner)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut1[i].x;
                    int itsY = boardLayOut1[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut1[i].x * .3f, boardLayOut1[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(DRCornerPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;
                    
                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                }
            }
        } else if(presetToUse == 2)
        {
            //look at all the tiles in the layout
            for (int i = 0; i < boardLayOut2.Length; i++)
            {
                //if a tile is a "Circuit" tile
                if (boardLayOut2[i].tileKind == TileKind.Vertical)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut2[i].x;
                    int itsY = boardLayOut2[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut2[i].x * .3f, boardLayOut2[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(VerticalPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;

                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                }
                else if (boardLayOut2[i].tileKind == TileKind.Horizontal)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut2[i].x;
                    int itsY = boardLayOut2[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut2[i].x * .3f, boardLayOut2[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(HorizontalPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;

                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                }
                else if (boardLayOut2[i].tileKind == TileKind.URCorner)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut2[i].x;
                    int itsY = boardLayOut2[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut2[i].x * .3f, boardLayOut2[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(URCornerPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;

                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                }
                else if (boardLayOut2[i].tileKind == TileKind.LDCorner)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut2[i].x;
                    int itsY = boardLayOut2[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut2[i].x * .3f, boardLayOut2[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(LDCornerPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;

                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                }
                else if (boardLayOut2[i].tileKind == TileKind.LUCorner)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut2[i].x;
                    int itsY = boardLayOut2[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut2[i].x * .3f, boardLayOut2[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(LUCornerPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;

                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                }
                else if (boardLayOut2[i].tileKind == TileKind.DRCorner)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut2[i].x;
                    int itsY = boardLayOut2[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut2[i].x * .3f, boardLayOut2[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(DRCornerPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;

                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                }
            }
        } else if(presetToUse == 3)
        {
            //look at all the tiles in the layout
            for (int i = 0; i < boardLayOut3.Length; i++)
            {
                //if a tile is a "Circuit" tile
                if (boardLayOut3[i].tileKind == TileKind.Vertical)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut3[i].x;
                    int itsY = boardLayOut3[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut3[i].x * .3f, boardLayOut3[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(VerticalPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;

                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                }
                else if (boardLayOut3[i].tileKind == TileKind.Horizontal)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut3[i].x;
                    int itsY = boardLayOut3[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut3[i].x * .3f, boardLayOut3[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(HorizontalPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;

                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                }
                else if (boardLayOut3[i].tileKind == TileKind.URCorner)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut3[i].x;
                    int itsY = boardLayOut3[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut3[i].x * .3f, boardLayOut3[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(URCornerPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;

                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                }
                else if (boardLayOut3[i].tileKind == TileKind.LDCorner)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut3[i].x;
                    int itsY = boardLayOut3[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut3[i].x * .3f, boardLayOut3[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(LDCornerPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;

                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                }
                else if (boardLayOut3[i].tileKind == TileKind.LUCorner)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut3[i].x;
                    int itsY = boardLayOut3[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut3[i].x * .3f, boardLayOut3[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(LUCornerPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;

                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                }
                else if (boardLayOut3[i].tileKind == TileKind.DRCorner)
                {
                    //create a "circuit" tile at that position
                    int itsX = boardLayOut3[i].x;
                    int itsY = boardLayOut3[i].y;
                    Vector3 tempPosition = new Vector3(boardLayOut3[i].x * .3f, boardLayOut3[i].y * .3f, 0.001f);
                    GameObject tile = Instantiate(DRCornerPrefab, tempPosition, Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.transform.localPosition = tempPosition;

                    circuitTilesGO[itsX, itsY] = tile;
                    circuitTilesCreated++;
                }
            }
        }
    }
}
