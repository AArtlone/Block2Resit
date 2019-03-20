using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageShake : MonoBehaviour {

    private float duration = 1.7f;
    private float shakeRange = 5f;
    private Vector2 startingPos;
    private SpriteRenderer mySR;

	// Use this for initialization
	void Start () {
        mySR = GetComponent<SpriteRenderer>();
    }
    private void Awake()
    {
        startingPos.x = transform.position.x;
        startingPos.y = transform.position.y;
    }

    // Update is called once per frame
    void Update () {
        if(ManaginAudio.audioLooping)
        {
            Debug.Log("trying");
            StartCoroutine(Shake());
        }
	}
    public void ShakeCo()
    {
        StartCoroutine(Shake());
    }
    private IEnumerator Shake()
    {
        float elapsed = 0.0f;
        Quaternion originalRotation = transform.rotation;

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;
            float z = Random.value * shakeRange - (shakeRange / 2);
            transform.eulerAngles = new Vector3(originalRotation.x, originalRotation.y + 90f, originalRotation.z + z);
            yield return null;
        }

        transform.rotation = originalRotation;
    }
}
