using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
    
    private ScreenFader sf;

    public IEnumerator DoTeleportation()
    {
        yield return StartCoroutine(sf.FadeToBlack());
        yield return StartCoroutine(sf.FadeToClear());
    }

    public void TeleportationCo()
    {
        StartCoroutine(DoTeleportation());
    }
    // Use this for initialization
    void Start()
    {
        //sf = GameObject.FindGameObjectWithTag("Fader").GetComponent<ScreenFader>();
    }
    
}
