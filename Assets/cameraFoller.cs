using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFoller : MonoBehaviour {

    public GameObject followTarget;
    
    // Update is called once per frame
    void Update() {
        this.transform.LookAt(followTarget.transform);
    }
}
