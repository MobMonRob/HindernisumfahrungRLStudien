using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDelayScript : MonoBehaviour {

    public float simulationSpeed = 0f;
    
    // Start is called before the first frame update
    void Start() {
        Time.timeScale = simulationSpeed;
    }

    // Update is called once per frame
    void Update() {
        Time.timeScale = simulationSpeed;
    }
}