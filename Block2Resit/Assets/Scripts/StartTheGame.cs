using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTheGame : MonoBehaviour {

    //public AudioClip instructionsSound;
    //public AudioSource audioSource;

    private IEnumerator StartGameCo()
    {
        yield return new WaitForSeconds(2f);
        FindObjectOfType<ManaginAudio>().PlayRinging();
    }

    // Use this for initialization
    void Start () {
        //audioSource.clip = instructionsSound;
        StartCoroutine(StartGameCo());
    }
}
