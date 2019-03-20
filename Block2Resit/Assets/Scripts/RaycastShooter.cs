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

    private Vector3 offSet = new Vector3(0f, 1f, 0f);

    public GameObject point;
    private ScreenFader sf;
    public GameObject player;
    public GameObject startingPos;
    public GameObject circuitPos;
    public GameObject sgPos;
    public GameObject introPos;
    public GameObject titleText0_2;
    public GameObject titleText1_2;
    public GameObject titleText2_2;
    public GameObject exitMission;
    public GameObject exitMissionDisabled;
    public GameObject calllImages;
    public GameObject bannerImages;
    public GameObject controlsImage;
    public GameObject medal;
    public GameObject laserNonPU;
    public GameObject laserPU;

    public _GameState currentState = _GameState.gameOff;

    private Vector3 downPosition;
    private Vector3 upPosition;
    public float swipeAngle;
    private GameObject hitToUse; // the intial dot that is being hit before the swiping is done

    public static bool circuitFinished = false;
    public static bool sgFinished = false;
    public static bool gameIsOn = false;
    public static int puzzlesFinished = 0;

    public GameObject button;
    public GameObject startGameButton;

    private Vector3 hitForTeleport;

    public void UpdateUI()
    {
        if(puzzlesFinished == 1)
        {
            titleText0_2.SetActive(false);
            titleText1_2.SetActive(true);
        } else if(puzzlesFinished == 2)
        {
            titleText1_2.SetActive(false);
            exitMissionDisabled.SetActive(false);
            titleText2_2.SetActive(true);
            exitMission.SetActive(true);
        }
        //puzzlesFinishedText1.text = puzzlesFinished.ToString("0");
        //puzzlesFinishedText2.text = puzzlesFinished.ToString("0");
        //if (puzzlesFinished == 1)
        //{
        //    feedbackText1.text = "You're almost there! Just one more to go!";
        //    feedbackText2.text = "You're almost there! Just one more to go!";
        //}
        //else if (puzzlesFinished == 2)
        //{
        //    feedbackText1.text = "Great, your energy bill is now payable! Remember, turning off the lights and radiator brings down your bill and saves energy!";
        //    feedbackText2.text = "Great, your energy bill is now payable! Remember, turning off the lights and radiator brings down your bill and saves energy!";
        //}
    }
    
    private void Start()
    {
        sf = FindObjectOfType<ScreenFader>();
        point.SetActive(false);
        //puzzlesFinishedText1.text = puzzlesFinished.ToString("0");
        //puzzlesFinishedText2.text = puzzlesFinished.ToString("0");
        //feedbackText1.text = "Your energy bill is too high, solve the puzzles to bring it down!";
        //feedbackText2.text = "Your energy bill is too high, solve the puzzles to bring it down!";
    }
    void Update ()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            //if (hit.collider.tag == "Green" || hit.collider.tag == "Yellow" || hit.collider.tag == "Pink" || hit.collider.tag == "Red" || hit.collider.tag == "Blue" || hit.collider.tag == "SG")
            //{
            //    hit.collider.GetComponent<Dot>().MovePieces();
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
            if (hit.collider.tag == "CircuitPos")
            {
                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                {
                    sf.TeleportationCo();
                    StartCoroutine(TeleportCircuitPosCo());
                }
            }
            if (hit.collider.tag == "SGPos")
            {
                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                {
                    sf.TeleportationCo();
                    StartCoroutine(TeleportSGPosCo());
                }
            }
            if (hit.collider.tag == "ExitMission")
            {
                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                {
                    sf.TeleportationCo();
                    StartCoroutine(TeleportIntroPosCo());
                }
            }
            if (hit.collider.tag == "StartGame")
            {
                if (Input.GetKeyUp("m"))
                {
                    sf.TeleportationCo();
                    StartCoroutine(TeleportStartPosCo());
                }
                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                {
                    sf.TeleportationCo();
                    startGameButton.GetComponent<Collider>().enabled = false;
                    StartCoroutine(TeleportStartPosCo());
                }
            }
            if (hit.collider.tag == "AcceptCall")
            {
                if (Input.GetKeyUp("m"))
                {
                    ManaginAudio.audioLooping = false;
                    FindObjectOfType<ManaginAudio>().audioSource.Stop();
                    calllImages.SetActive(false);
                    bannerImages.SetActive(true);
                    Invoke("PlayInstructionsInv", 1f);
                }
                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                {
                    ManaginAudio.audioLooping = false;
                    FindObjectOfType<ManaginAudio>().audioSource.Stop();
                    calllImages.SetActive(false);
                    bannerImages.SetActive(true);
                    Invoke("PlayInstructionsInv", 1f);
                }
            }
            if (hit.collider.tag == "DeclineCall")
            {
                if (Input.GetKeyUp("m"))
                {
                    ManaginAudio.audioLooping = false;
                    FindObjectOfType<ManaginAudio>().audioSource.Stop();
                    Invoke("PlayRingingAgain", 2f);
                }
                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                {
                    ManaginAudio.audioLooping = false;
                    FindObjectOfType<ManaginAudio>().audioSource.Stop();
                    Invoke("PlayRingingAgain", 2f);
                }
            }
            if (hit.collider.tag == "Teleportable")
            {
                //removableCube.SetActive(true);
                point.SetActive(true);
                point.transform.position = hit.point;
                //call a function that causes teleportation
                if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
                {
                    hitForTeleport = hit.point;
                    sf.TeleportationCo();
                    StartCoroutine(TeleportCo());
                    point.SetActive(false);
                }
            } else
            {
                point.SetActive(false);
            }
        }
    }
    private IEnumerator TeleportCo()
    {
        yield return new WaitForSeconds(1f);
        player.transform.position = hitForTeleport + offSet;
    }
    private IEnumerator TeleportStartPosCo()
    {
        FindObjectOfType<ManaginAudio>().PLayIntro();
        yield return new WaitForSeconds(1f);
        player.transform.position = startingPos.transform.position;
        player.transform.rotation = startingPos.transform.rotation;
        startGameButton.SetActive(false);
        controlsImage.SetActive(false);
        medal.SetActive(true);
        laserNonPU.SetActive(false);
        laserPU.SetActive(true);
        //bannerImages.SetActive(false);
    }
    private IEnumerator TeleportCircuitPosCo()
    {
        yield return new WaitForSeconds(1f);
        player.transform.position = circuitPos.transform.position;
        player.transform.rotation = circuitPos.transform.rotation;
    }
    private IEnumerator TeleportSGPosCo()
    {
        yield return new WaitForSeconds(1f);
        player.transform.position = sgPos.transform.position;
        player.transform.rotation = sgPos.transform.rotation;
    }
    private IEnumerator TeleportIntroPosCo()
    {
        yield return new WaitForSeconds(1f);
        player.transform.position = introPos.transform.position;
        player.transform.rotation = introPos.transform.rotation;
        FindObjectOfType<ManaginAudio>().PLayMissionFinished();
    }
    private void PlayRingingAgain()
    {
        FindObjectOfType<ManaginAudio>().PlayRinging();
    }
    private void PlayInstructionsInv()
    {
        FindObjectOfType<ManaginAudio>().PlayInstructions();
    }
}
