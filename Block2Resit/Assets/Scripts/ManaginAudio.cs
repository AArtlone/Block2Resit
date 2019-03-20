using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaginAudio : MonoBehaviour {
    
    public AudioClip winSoundCircuit;
    public AudioClip winSoundSG;
    public AudioClip instructionsSound;
    public AudioClip introSound;
    public AudioClip missionFinished;
    public AudioClip clapping;
    public AudioClip ringing;
    public AudioClip finishPuzzle;
    public AudioClip makeMatch;
    public AudioClip swiping;
    public AudioClip sgExplained;
    public AudioClip circuitExplained;
    public AudioSource audioSource;

    public static bool audioLooping = false;

    public GameObject startGameButton;
    public GameObject startGameButtonDis;
    public GameObject laserNonPU;
    public GameObject bannerImages;

    private void Update()
    {   

    }
    public void PlaySGExplain()
    {
        audioSource.PlayOneShot(sgExplained, 1f);
    }
    public void PlayCircuitExplain()
    {
        audioSource.PlayOneShot(circuitExplained, 1f);
    }
    public void PlayMakeMatch()
    {
        audioSource.PlayOneShot(makeMatch, 1f);
    }
    

    public void PlaySwiping()
    {
        audioSource.PlayOneShot(swiping, 1f);
    }

    public void PLayFinishPuzzle()
    {
        audioSource.PlayOneShot(finishPuzzle, 1f);
    }

    public void PlayRinging()
    {
        //make it loop
        audioLooping = true;
        audioSource.loop = true;
        audioSource.clip = ringing;
        audioSource.volume = .8f;
        audioSource.Play();
        FindObjectOfType<ImageShake>().ShakeCo();
    }

    public void PLayMissionFinished()
    {
        audioSource.PlayOneShot(missionFinished, 1f);
        StartCoroutine(ClappingCo());
    }

    public void PlayCircuitWinSound()
    {
        StartCoroutine(CircuitWinCo());
    }

    public void PlaySGWinSound()
    {
        StartCoroutine(SGWinCo());
    }

    public void PLayIntro()
    {
        audioSource.PlayOneShot(introSound, 1f);
    }

    public void PlayInstructions()
    {
        audioSource.PlayOneShot(instructionsSound, 1f);
        StartCoroutine(EnableStartButtonCo());
    }
    private IEnumerator EnableStartButtonCo()
    {
        yield return new WaitForSeconds(10f);
        bannerImages.SetActive(false);
        laserNonPU.SetActive(true);
        yield return new WaitForSeconds(20f);
        startGameButtonDis.SetActive(true);
        yield return new WaitForSeconds(8f);
        startGameButtonDis.SetActive(false);
        startGameButton.SetActive(true);
        startGameButton.GetComponent<Collider>().enabled = true;
    }
    private IEnumerator ClappingCo()
    {
        yield return new WaitForSeconds(16f);
        audioSource.PlayOneShot(clapping, 1f);
    }
    private IEnumerator SGWinCo()
    {
        yield return new WaitForSeconds(1f);
        audioSource.PlayOneShot(winSoundSG, 1f);
    }
    private IEnumerator CircuitWinCo()
    {
        yield return new WaitForSeconds(1f);
        audioSource.PlayOneShot(winSoundCircuit, 1f);
    }
}
