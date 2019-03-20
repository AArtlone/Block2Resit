using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DotSG : MonoBehaviour {

    [Header("Board variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;

    private FindMatchesSG findMatches;
    private BoardSG board;
    private RaycastShooter raycast;
    private GameObject otherDot;
    private Vector2 tempPosition; //tmeporarily hold the target position that the target needs to move to


    private void Start()
    {
        board = FindObjectOfType<BoardSG>();
        findMatches = FindObjectOfType<FindMatchesSG>();
        raycast = FindObjectOfType<RaycastShooter>();
        row = targetY;
        column = targetX;
    }

    private void Update()
    {
        //this change this Dot's x or y according to changes done after swiping in MovePieces()
        targetX = column;
        targetY = row;

        ActualMovePieces();
        //CheckingForMatches();
    }

    //swaps 2 Dots positions
    private void ActualMovePieces()
    {
        //checks if there is a difference betweeen the actual position and the needed position of each dot
        if (Mathf.Abs(targetX * .3f - transform.localPosition.x) > .1)
        {
            //move towards the target
            tempPosition = new Vector3(targetX * .3f, transform.localPosition.y, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, tempPosition, .1f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
            //directly set the position
            tempPosition = new Vector3(targetX * .3f, transform.localPosition.y, 0);
            transform.localPosition = tempPosition;
        }
        if (Mathf.Abs(targetY * .3f - transform.position.y) > .1)
        {
            //move towards the target
            tempPosition = new Vector3(transform.localPosition.x, targetY * .3f, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, tempPosition, .1f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
            //directly set the position
            tempPosition = new Vector3(transform.localPosition.x, targetY * .3f);
            transform.localPosition = tempPosition;
        }
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);
        if (otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<DotSG>().isMatched)
            {
                otherDot.GetComponent<DotSG>().row = row;
                otherDot.GetComponent<DotSG>().column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.5f);
                board.currentState = GameStateSG.move;
            }
            else
            {
                board.DestroyMatches();
            }
            otherDot = null;
        }
    }

    //this is being called when raycast hits the Dot and check for which swipe was done
    public void MovePieces()
    {
        if (board.currentState == GameStateSG.move)
        {
            if (raycast.swipeAngle > -45 && raycast.swipeAngle <= 45 && column < board.width - 1)
            {
                FindObjectOfType<ManaginAudio>().PlaySwiping();
                board.currentState = GameStateSG.wait;
                //right swipe; changing the other dot's(dot to the right of us) column to -1 while changing our current dot column to +1 
                otherDot = board.allDots[column + 1, row];
                previousRow = row;
                previousColumn = column;
                otherDot.GetComponent<DotSG>().column -= 1;
                column += 1;
                StartCoroutine(CheckMoveCo());
            }
            else if (raycast.swipeAngle > 45 && raycast.swipeAngle <= 135 && row < board.height - 1)
            {
                FindObjectOfType<ManaginAudio>().PlaySwiping();
                board.currentState = GameStateSG.wait;
                //up swipe; changing the other dot's(dot to the right of us) row to -1 while changing our current dot row to +1 
                otherDot = board.allDots[column, row + 1];
                previousRow = row;
                previousColumn = column;
                otherDot.GetComponent<DotSG>().row -= 1;
                row += 1;
                StartCoroutine(CheckMoveCo());
            }
            else if ((raycast.swipeAngle > 135 || raycast.swipeAngle <= -135) && column > 0)
            {
                FindObjectOfType<ManaginAudio>().PlaySwiping();
                board.currentState = GameStateSG.wait;
                //left swipe; changing the other dot's(dot to the right of us) column to +1 while changing our current dot column to 11 
                otherDot = board.allDots[column - 1, row];
                previousRow = row;
                previousColumn = column;
                otherDot.GetComponent<DotSG>().column += 1;
                column -= 1;
                StartCoroutine(CheckMoveCo());
            }
            else if (raycast.swipeAngle < -45 && raycast.swipeAngle >= -135 && row > 0)
            {
                FindObjectOfType<ManaginAudio>().PlaySwiping();
                board.currentState = GameStateSG.wait;
                //down swipe; changing the other dot's(dot to the right of us) row to +1 while changing our current dot row to -1 
                otherDot = board.allDots[column, row - 1];
                previousRow = row;
                previousColumn = column;
                otherDot.GetComponent<DotSG>().row += 1;
                row -= 1;
                StartCoroutine(CheckMoveCo());
            }

            //if (Input.GetKey("l"))
            //{
            //    board.currentState = GameStateSG.wait;
            //    //right swipe; changing the other dot's(dot to the right of us) column to -1 while changing our current dot column to +1 
            //    otherDot = board.allDots[column + 1, row];
            //    previousRow = row;
            //    previousColumn = column;
            //    otherDot.GetComponent<DotSG>().column -= 1;
            //    column += 1;
            //    StartCoroutine(CheckMoveCo());
            //}
            //else if (Input.GetKey("i"))
            //{
            //    board.currentState = GameStateSG.wait;
            //    //up swipe; changing the other dot's(dot to the right of us) row to -1 while changing our current dot row to +1 
            //    otherDot = board.allDots[column, row + 1];
            //    previousRow = row;
            //    previousColumn = column;
            //    otherDot.GetComponent<DotSG>().row -= 1;
            //    row += 1;
            //    StartCoroutine(CheckMoveCo());
            //}
            //else if (Input.GetKey("j"))
            //{
            //    board.currentState = GameStateSG.wait;
            //    //left swipe; changing the other dot's(dot to the right of us) column to +1 while changing our current dot column to 11 
            //    otherDot = board.allDots[column - 1, row];
            //    previousRow = row;
            //    previousColumn = column;
            //    otherDot.GetComponent<DotSG>().column += 1;
            //    column -= 1;
            //    StartCoroutine(CheckMoveCo());
            //}
            //else if (Input.GetKey("k"))
            //{
            //    board.currentState = GameStateSG.wait;
            //    //down swipe; changing the other dot's(dot to the right of us) row to +1 while changing our current dot row to -1 
            //    otherDot = board.allDots[column, row - 1];
            //    previousRow = row;
            //    previousColumn = column;
            //    otherDot.GetComponent<DotSG>().row += 1;
            //    row -= 1;
            //    StartCoroutine(CheckMoveCo());
            //}
        }
    }

    //checks if there are any matched Dots
    void CheckingForMatches()
    {
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(0f, 0f, 0f, .2f);
        }
    }

    private bool SpecialGemCheck()
    {
        if (this.gameObject.tag == "Special")
        {
            if (row <= 0)
            {
                isMatched = true;
                //Board.specialGemsCollected++;
            }
        }
        return false;
    }
}
