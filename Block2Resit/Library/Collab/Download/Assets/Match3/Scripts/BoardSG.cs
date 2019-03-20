using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStateSG
{
    wait,
    move
}

public class BoardSG : MonoBehaviour
{

    public GameStateSG currentState = GameStateSG.move;
    private bool _gameStarted = false;

    public int width;
    public int height;
    public int offSet;
    public GameObject tilePrefab;

    private BackgroundTile[,] allTiles;
    public GameObject[,] allDots;
    public GameObject[] dots;
    private FindMatchesCircuit findMatches;
    private RaycastShooter raycast;

    private int specialGemIterations = 0;
    public static int specialGemsCollected = 0;
    public GameObject _canvas;
    public GameObject sgCanvas; //canvas that display sg info
    public Text sgText;
    public GameObject tick;
    public GameObject cross;

    public static int sgCollected = 0;
    public static int sgNeeded = 3;
    private int sgOnBoard = 0;

    // Use this for initialization
    void Start()
    {
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        findMatches = FindObjectOfType<FindMatchesCircuit>();
        raycast = FindObjectOfType<RaycastShooter>();
        //SetUpSpecialGem();
    }

    //private void Update()
    //{
    //    CheckForGameOver();
    //}

    public void UpdateSGCanvas()
    {
        sgText.text = sgCollected.ToString("0");
    }

    private IEnumerator SGCanvasCo()
    {
        yield return new WaitForSeconds(1f);
        sgCanvas.gameObject.SetActive(true);
        sgText.text = sgCollected.ToString("0");
    }

    //public void CheckForGameOver()
    //{
    //    if (raycast.currentState == _GameState.gameOff)
    //    {
    //        StartCoroutine(GameOverCo());
    //    }
    //}

    public IEnumerator GameOverCo()
    {
        yield return new WaitForSeconds(1f);
        _canvas.SetActive(false);
        sgCanvas.SetActive(false);
        //play sound
    }

    public void SetUpSpecialGem()
    {
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
                    while (dots[dotToUse].gameObject.tag == "SG")
                    {
                        dotToUse = Random.Range(0, dots.Length);
                    }
                    while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                    {
                        dotToUse = Random.Range(0, dots.Length);
                        maxIterations++;
                    }
                    maxIterations = 0;
                    GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    dot.GetComponent<DotSG>().targetX = i;
                    dot.GetComponent<DotSG>().targetY = j;
                    dot.transform.parent = this.transform;
                    dot.transform.localPosition = tempPosition;
                    dot.name = "( " + i + ", " + j + " )";
                    allDots[i, j] = dot;
                }
            }
            StartCoroutine(SGCanvasCo());
        }
    }

    public void ShuffleBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Destroy(allDots[i, j]);
                if(allDots[i, j].tag == "SG")
                {
                    sgOnBoard--;
                }
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
                while (dots[dotToUse].gameObject.tag == "SG")
                {
                    dotToUse = Random.Range(0, dots.Length);
                }
                while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, dots.Length);
                    maxIterations++;
                }
                maxIterations = 0;
                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<DotSG>().targetX = i;
                dot.GetComponent<DotSG>().targetY = j;
                dot.transform.parent = this.transform;
                dot.transform.localPosition = tempPosition;
                dot.name = "( " + i + ", " + j + " )";
                allDots[i, j] = dot;
            }
        }
    }

    //checks for matches while generating board
    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
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

        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
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
        if (allDots[column, row].GetComponent<DotSG>().isMatched)
        {
            findMatches.currentMatches.Remove(allDots[column, row]);
            Destroy(allDots[column, row]);
            if(allDots[column, row].tag == "SG")
            {
                sgCollected++;
                sgOnBoard--;
                UpdateSGCanvas();
                if(sgCollected == sgNeeded)
                {
                    raycast.currentState = _GameState.gameOff;
                    currentState = GameStateSG.wait;
                    StartCoroutine(GameOverCo());
                    RaycastShooter.sgFinished = true;
                    cross.SetActive(false);
                    tick.SetActive(true);
                    RaycastShooter.puzzlesFinished++;
                    raycast.UpdateUI();
                }
            }
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
                    allDots[i, j].GetComponent<DotSG>().row -= nullCount;
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
                    if (sgOnBoard >= 3 && dots[dotToUse].gameObject.tag == "SG")
                    {
                        dotToUse = Random.Range(0, dots.Length);
                    }
                    while (dots[dotToUse].gameObject.name == "SpecialGem" && specialGemIterations <= 20)
                    {
                        specialGemIterations++;
                        dotToUse = Random.Range(0, dots.Length);
                    }
                    GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    if(dot.tag == "SG")
                    {
                        sgOnBoard++;
                    }
                    dot.GetComponent<DotSG>().targetX = i;
                    dot.GetComponent<DotSG>().targetY = j;
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
                    if (allDots[i, j].GetComponent<DotSG>().isMatched)
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
        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        yield return new WaitForSeconds(1f);
        currentState = GameStateSG.move;
    }
}
