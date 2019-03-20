using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialGemMatch3Button : MonoBehaviour {

    private RaycastShooter raycast;
    public GameObject _canvas;
    public GameObject shuffleButton;

    public GameObject turnOffRadiatorButton;
    public GameObject turnOffRadiatorDisButton;

    private void Start()
    {
        raycast = FindObjectOfType<RaycastShooter>();
    }

    public void CheckForInput()
    {
        raycast.currentState = _GameState.gameOn;
        if (raycast.currentState == _GameState.gameOn && !RaycastShooter.sgFinished)
        {
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) && !RaycastShooter.gameIsOn)
            {
                FindObjectOfType<ManaginAudio>().PlaySGExplain();
                RaycastShooter.gameIsOn = true;
                _canvas.SetActive(true);
                shuffleButton.SetActive(true);
                FindObjectOfType<BoardSG>().SetUpSpecialGem();
                GetComponent<Collider>().enabled = false;
                Invoke("SwapButtons", .5f);
            }
            if (Input.GetKeyUp("m"))
            {
                RaycastShooter.gameIsOn = true;
                Debug.Log("sd");
                _canvas.SetActive(true);
                FindObjectOfType<BoardSG>().SetUpSpecialGem();
            }
        }
    }
    
    public void SwapButtons()
    {
        turnOffRadiatorDisButton.SetActive(true);
        turnOffRadiatorButton.SetActive(false);
    }
}
