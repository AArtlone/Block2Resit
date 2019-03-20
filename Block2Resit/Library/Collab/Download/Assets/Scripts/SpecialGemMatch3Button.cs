using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialGemMatch3Button : MonoBehaviour {

    private RaycastShooter raycast;
    public GameObject _canvas;

    private void Start()
    {
        raycast = FindObjectOfType<RaycastShooter>();
    }

    public void CheckForInput()
    {
        raycast.currentState = _GameState.gameOn;
        if (raycast.currentState == _GameState.gameOn && !RaycastShooter.sgFinished)
        {
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
            {
                _canvas.SetActive(true);
                FindObjectOfType<BoardSG>().SetUpSpecialGem();
                GetComponent<Collider>().enabled = false;
            }
            if (Input.GetKeyUp("m"))
            {
                Debug.Log("sd");
                _canvas.SetActive(true);
                FindObjectOfType<BoardSG>().SetUpSpecialGem();
            }
        }
    }
}
