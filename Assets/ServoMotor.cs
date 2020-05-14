using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServoMotor : MonoBehaviour {

    public float currentAngle = 0;
    public float targetAngle = 0;
    public float lastAngle = 0;
    public float rotationSpeed = 120; //in degrees per second
    public float lowerLimit = -60f;
    public float upperLimit = 60f;
    
    private HingeJoint joint;

    // Start is called before the first frame update
    void Start() {
        joint = this.GetComponent<HingeJoint>();
        joint.useLimits = true;
        var motor = joint.motor;
        motor.targetVelocity = 0f;
        motor.force = 100f;
        joint.motor = motor;
    }

    // Update is called once per frame
    void FixedUpdate() {
        var limits = joint.limits;
        var maxRotate = rotationSpeed * Time.deltaTime;

        var angleDiff = targetAngle - currentAngle;
        var toRotate = angleDiff;
        if (angleDiff > maxRotate) toRotate = maxRotate;
        if (angleDiff < -maxRotate) toRotate = -maxRotate;
        
        var newAngle = currentAngle + toRotate;

        if (newAngle > upperLimit) newAngle = upperLimit;
        if (newAngle < lowerLimit) newAngle = lowerLimit;

        limits.min = newAngle - 1;
        limits.max = newAngle + 1;

        lastAngle = currentAngle;
        currentAngle = newAngle;
        
        joint.limits = limits;

    }

    public void reset() {
        this.currentAngle = 0;
        this.targetAngle = 0;
        FixedUpdate();
    }
}