using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum _GameState
{
    gameOn,
    gameOff
}

public class RaycastShooter : MonoBehaviour {

    public GameObject point;
    private Teleport teleport;

    public _GameState currentState = _GameState.gameOff;

    private Vector3 downPosition;
    private Vector3 upPosition;
    public float swipeAngle;
    private GameObject hitToUse; // the intial dot that is being hit before the swiping is done

    public static bool circuitFinished = false;
    public static bool sgFinished = false;
    public static int puzzlesFinished = 0;
    
    public Text puzzlesFinishedText1;
    public Text puzzlesFinishedText2;
    public Text feedbackText1;
    public Text feedbackText2;

    public GameObject button;

    public void UpdateUI()
    {
        puzzlesFinishedText1.text = puzzlesFinished.ToString("0");
        puzzlesFinishedText2.text = puzzlesFinished.ToString("0");
        if (puzzlesFinished == 1)
        {
            feedbackText1.text = "You're almost there! Just one more to go!";
            feedbackText2.text = "You're almost there! Just one more to go!";
        }
        else if (puzzlesFinished == 2)
        {
            feedbackText1.text = "Great, your energy bill is now payable! Remember, turning off the lights and radiator brings down your bill and saves energy!";
            feedbackText2.text = "Great, your energy bill is now payable! Remember, turning off the lights and radiator brings down your bill and saves energy!";
        }
    }
    
    private void Start()
    {
        teleport = FindObjectOfType<Teleport>();
        //point.transform.parent = GameObject.FindGameObjectWithTag("ControllerPointer").transform;
        point.SetActive(false);
        puzzlesFinishedText1.text = puzzlesFinished.ToString("0");
        puzzlesFinishedText2.text = puzzlesFinished.ToString("0");
        feedbackText1.text = "Your energy bill is too high, solve the puzzles to bring it down!";
        feedbackText2.text = "Your energy bill is too high, solve the puzzles to bring it down!";
    }
    void Update ()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            //if (hit.collider.tag == "GreenSG" || hit.collider.tag == "YellowSG" || hit.collider.tag == "PinkSG" || hit.collider.tag == "RedSG" || hit.collider.tag == "BlueSG" || hit.collider.tag == "SG")
            //{
            //    hit.collider.GetComponent<DotSG>().MovePieces();
            //}

            if (currentState == _GameState.gameOn && (hit.collider.tag == "Green" || hit.collider.tag == "Yellow" || hit.collider.tag == "Pink" || hit.collider.tag == "Red" || hit.collider.tag == "Blue"))
            {
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                {
                    downPosition = hit.point;
                    //downPosText.text = downPosition.ToString();
                    hitToUse = hit.collider.gameObject;
                }
                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                {
                    upPosition = hit.point;
                   // upPosText.text = upPosition.ToString();
                    swipeAngle = Mathf.Atan2(upPosition.y - downPosition.y, upPosition.x - downPosition.x) * 180 / Mathf.PI;
                    //angleText.text = swipeAngle.ToString();
                    hitToUse.GetComponent<Dot>().MovePieces();
                }
            }


            if (currentState == _GameState.gameOn && (hit.collider.tag == "GreenSG" || hit.collider.tag == "YellowSG" || hit.collider.tag == "PinkSG" || hit.collider.tag == "RedSG" || hit.collider.tag == "BlueSG" || hit.collider.tag == "SG"))
            {
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                {
                    downPosition = hit.point;
                    //downPosText.text = downPosition.ToString();
                    hitToUse = hit.collider.gameObject;
                }
                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                {
                    upPosition = hit.point;
                    // upPosText.text = upPosition.ToString();
                    swipeAngle = Mathf.Atan2(upPosition.y - downPosition.y, upPosition.x - downPosition.x) * 180 / Mathf.PI;
                    //angleText.text = swipeAngle.ToString();
                    hitToUse.GetComponent<DotSG>().MovePieces();
                }
            }
            if (hit.collider.tag == "StartCircuit")
            {
                currentState = _GameState.gameOn;
                hit.collider.GetComponent<CircuitMatch3Button>().CheckForInput();
            }
            if (hit.collider.tag == "StartSpecialGem")
            {
                currentState = _GameState.gameOn;
                hit.collider.GetComponent<SpecialGemMatch3Button>().CheckForInput();
            }
            if (hit.collider.tag == "ShuffleCircuit")
            {
                hit.collider.GetComponent<ShuffleCircuit>().CheckForInput();
            }
            if (hit.collider.tag == "ShuffleSG")
            {
                hit.collider.GetComponent<ShuffleSG>().CheckForInput();
            }
            if (hit.collider.name == "TakeMeToTheCircuit?")
            {
                //teleport to the circuit position
            }
            if (hit.collider.name == "TakeMeToTheSG?")
            {
                //teleport to the SG position
            }
            //if (hit.collider.tag == "ShuffleButton")
            //{
            //    hit.collider.GetComponent<ShuffleButton>().ShuffleMatch3();
            //}
            //if (hit.collider.tag == "Teleportable")
            //{
            //    //removableCube.SetActive(true);
            //    point.SetActive(true);
            //    point.transform.position = hit.point;
            //    //call a function that causes teleportation
            //    if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
            //    {
            //        teleport.DoTeleportation();                    
            //    }
            //} else
            //{
            //    //point.SetActive(false);
            //}
        }

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        //{
        //    if (hit.collider.tag == "Pointable")
        //    {
        //        removableCube.SetActive(true);
        //        point.SetActive(true);
        //        point.transform.position = hit.point;
        //    } 
        //    if (hit.collider.tag == "Teleportable")
        //    {
        //        removableCube.SetActive(true);
        //        point.SetActive(true);
        //        point.transform.position = hit.point;
        //        //call a function that causes teleportation
        //        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        //        {
        //            teleport.DoTeleportation();
        //        }
        //    }
        //    if (hit.collider.tag == "Floor")
        //    {
        //        removableCube.SetActive(false);
        //        point.SetActive(false);
        //    }
        //}
        //else
        //{
        //    removableCube.SetActive(false);
        //    point.SetActive(false);
        //}
    }
}
