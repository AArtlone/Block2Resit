using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitMatch3Button : MonoBehaviour {

    private RaycastShooter raycast;
    public GameObject _canvas;
    public GameObject shuffleButton;

    public GameObject turnOffLightsButton;
    public GameObject turnOffLightsDisButton;

    private void Start()
    {
        raycast = FindObjectOfType<RaycastShooter>();
    }

    public void CheckForInput()
    {
        //raycast.currentState = _GameState.gameOn;
        if (raycast.currentState == _GameState.gameOn && !RaycastShooter.circuitFinished)
        {
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) && !RaycastShooter.gameIsOn)
            {
                FindObjectOfType<ManaginAudio>().PlayCircuitExplain();
                RaycastShooter.gameIsOn = true;
                _canvas.SetActive(true);
                shuffleButton.SetActive(true);
                FindObjectOfType<BoardCircuit>().SetUpCircuit();
                GetComponent<Collider>().enabled = false;
                Invoke("SwapButtons", .5f);
            }
            if (Input.GetKeyUp("m"))
            {
                RaycastShooter.gameIsOn = true;
                Debug.Log("sd");
                _canvas.SetActive(true);
                FindObjectOfType<BoardCircuit>().SetUpCircuit();
            }
        }
    }
    public void SwapButtons()
    {
        turnOffLightsDisButton.SetActive(true);
        turnOffLightsButton.SetActive(false);
    }
}
