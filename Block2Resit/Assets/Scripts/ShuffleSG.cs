using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleSG : MonoBehaviour {

    private RaycastShooter raycast;

    private void Start()
    {
        raycast = FindObjectOfType<RaycastShooter>();
    }

    public void CheckForInput()
    {
        if (raycast.currentState == _GameState.gameOn)
        {
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
            {
                FindObjectOfType<BoardSG>().ShuffleBoard();
            }
            if (Input.GetKey("m"))
            {
                FindObjectOfType<BoardSG>().ShuffleBoard();
            }
        }
    }
}
