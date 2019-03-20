using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGManager : MonoBehaviour {

    void Update () {
		if(GetComponent<DotSG>().row == 0)
        {
            GetComponent<DotSG>().isMatched = true;
        }
	}
}
