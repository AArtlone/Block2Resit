using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatchesSG : MonoBehaviour {

    private BoardSG board;
    private RaycastShooter raycast;
    public List<GameObject> currentMatchesSG = new List<GameObject>();


    // Use this for initialization
    void Start()
    {
        board = FindObjectOfType<BoardSG>();
        raycast = FindObjectOfType<RaycastShooter>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentDot = board.allDots[i, j];
                if (currentDot != null)
                {
                    //if (currentDot.gameObject.tag == "SG")
                    //{
                    //    if(currentDot.GetComponent<DotSG>().row == 0)
                    //    {
                    //        currentDot.GetComponent<DotSG>().isMatched = true;
                    //        BoardSG.sgCollected++;
                    //    }
                    //}
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if (leftDot != null && rightDot != null && leftDot.tag != "SG" && rightDot.tag != "SG")
                        {
                            if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {
                                if (!currentMatchesSG.Contains(leftDot))
                                {
                                    currentMatchesSG.Add(leftDot);
                                }
                                if (!currentMatchesSG.Contains(rightDot))
                                {
                                    currentMatchesSG.Add(rightDot);
                                }
                                if (!currentMatchesSG.Contains(currentDot))
                                {
                                    currentMatchesSG.Add(currentDot);
                                }
                                leftDot.GetComponent<DotSG>().isMatched = true;
                                rightDot.GetComponent<DotSG>().isMatched = true;
                                currentDot.GetComponent<DotSG>().isMatched = true;
                                //FindObjectOfType<ManaginAudio>().PlayMakeMatch();
                            }
                        }
                    }
                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upDot = board.allDots[i, j + 1];
                        GameObject downDot = board.allDots[i, j - 1];
                        if (upDot != null && downDot != null && upDot.tag != "SG" && downDot.tag != "SG")
                        {
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                            {
                                if (!currentMatchesSG.Contains(upDot))
                                {
                                    currentMatchesSG.Add(upDot);
                                }
                                if (!currentMatchesSG.Contains(downDot))
                                {
                                    currentMatchesSG.Add(downDot);
                                }
                                if (!currentMatchesSG.Contains(currentDot))
                                {
                                    currentMatchesSG.Add(currentDot);
                                }
                                downDot.GetComponent<DotSG>().isMatched = true;
                                upDot.GetComponent<DotSG>().isMatched = true;
                                currentDot.GetComponent<DotSG>().isMatched = true;
                                //FindObjectOfType<ManaginAudio>().PlayMakeMatch();
                            }
                        }
                    }
                }
            }
        }
    }
}
