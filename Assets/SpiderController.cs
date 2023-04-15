using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpiderController : MonoBehaviour {

    public Vector3 forwardDir;

    public ServoMotor center_front_right;
    public ServoMotor center_front_left;
    public ServoMotor center_back_right;
    public ServoMotor center_back_left;
    public ServoMotor middle_front_right;
    public ServoMotor middle_front_left;
    public ServoMotor middle_back_right;
    public ServoMotor middle_back_left;
    public ServoMotor outer_front_right;
    public ServoMotor outer_front_left;
    public ServoMotor outer_back_right;
    public ServoMotor outer_back_left;
    public ServoMotor[] allServos = new ServoMotor[12];

    public Transform center;
    
    private Vector3[] startPositions = new Vector3[13];
    private float initialX;
    private float lastProg = 0f;
    private Quaternion[] startOrientation = new Quaternion[13];

    private Vector3 lastPosition;
    private Vector3[] lastPositions = new Vector3[13];

    private void Start() {
        storeStart();
        initialX = center.position.x;
        lastPosition = center.position;

        allServos[0] = center_front_right;
        allServos[1] = center_front_left;
        allServos[2] = center_back_right;
        allServos[3] = center_back_left;
        allServos[4] = middle_front_right;
        allServos[5] = middle_front_left;
        allServos[6] = middle_back_right;
        allServos[7] = middle_back_left;
        allServos[8] = outer_front_right;
        allServos[9] = outer_front_left;
        allServos[10] = outer_back_right;
        allServos[11] = outer_back_left;

    }

    public void updatePositions() {
        lastPosition = center.position;
        for (var i = 0; i < 12; i++) {
            lastPositions[i] = allServos[i].transform.position;
        }
        lastPositions[12] = center.position;
    }

    public Vector3 getCenterPosition() {
        return wipeY(center.position);
    }

    public Vector3 getAvgPosition() {
        Vector3 sumPositions = Vector3.zero;
        Vector3 avgPosition = Vector3.zero;

        foreach (var servo in allServos) {
            sumPositions += servo.transform.position;
        }

        sumPositions += center.transform.position;
        avgPosition = sumPositions / 13;
        return avgPosition;
    }
    
    public float getCenterProgress() {
        return Vector3.Distance(wipeY(center.position), wipeY(lastPosition));
    }

    private float lastDistance = float.MaxValue;

    public float getAvgProgressToTarget(Vector3 targetPosition) {
        var distanceToTarget = Vector3.Distance(getAvgPosition(), targetPosition);
        var progress = lastDistance - distanceToTarget;
        lastDistance = distanceToTarget; // this is a problem, may only be called once per update !!!
        return progress;
    }

    private Vector3 wipeY(Vector3 vec) {
        vec.y = 0;
        return vec;
    }

    public Vector3 getAvgDirection() {
        Vector3 sumDirections = Vector3.zero;
        Vector3 avgDirection = Vector3.zero;
        for (var i = 0; i < allServos.Length; i++) {
            sumDirections += allServos[i].transform.position - lastPositions[i];
        }
        sumDirections += center.transform.position - lastPositions[12];
        avgDirection = sumDirections / 13;
        return avgDirection;
    }

    public Vector3 getCenterDirection() {
        return wipeY(center.position) - wipeY(lastPosition);
    }

    public Vector3 getAvgSpeed() {
        Vector3 velSum = Vector3.zero;
        Vector3 avgVel = Vector3.zero;

        int numOfRb = 0;
        foreach (var servo in allServos) {
            numOfRb++;
            var rb = servo.transform.GetComponent<Rigidbody>();
            velSum += rb.velocity;
        }

        numOfRb++;
        var rbCenter = center.transform.GetComponent<Rigidbody>();
        velSum += rbCenter.velocity;

        avgVel = velSum / numOfRb;
        return avgVel;
    }

    public bool isTurned() {
        var up = center.forward;
        var angle = Vector3.Angle(up, Vector3.up);
        return angle > 90;
    }

    public float getAngle() {
        var up = center.forward;
        var angle = Vector3.Angle(up, Vector3.up);
        return angle % 90;
    }

    private void storeStart() {
        startPositions[0] = center_front_left.transform.position;
        startPositions[1] = center_front_right.transform.position;
        startPositions[2] = center_back_left.transform.position;
        startPositions[3] = center_back_right.transform.position;
        startPositions[4] = middle_front_left.transform.position;
        startPositions[5] = middle_front_right.transform.position;
        startPositions[6] = middle_back_left.transform.position;
        startPositions[7] = middle_back_right.transform.position;
        startPositions[8] = outer_front_left.transform.position;
        startPositions[9] = outer_front_right.transform.position;
        startPositions[10] = outer_back_left.transform.position;
        startPositions[11] = outer_back_right.transform.position;
        startPositions[12] = center.transform.position;

        for(var i = 0; i < lastPositions.Length; i++) {
            lastPositions[i] = startPositions[i];
        }
        
        startOrientation[0] = center_front_left.transform.rotation;
        startOrientation[1] = center_front_right.transform.rotation;
        startOrientation[2] = center_back_left.transform.rotation;
        startOrientation[3] = center_back_right.transform.rotation;
        startOrientation[4] = middle_front_left.transform.rotation;
        startOrientation[5] = middle_front_right.transform.rotation;
        startOrientation[6] = middle_back_left.transform.rotation;
        startOrientation[7] = middle_back_right.transform.rotation;
        startOrientation[8] = outer_front_left.transform.rotation;
        startOrientation[9] = outer_front_right.transform.rotation;
        startOrientation[10] = outer_back_left.transform.rotation;
        startOrientation[11] = outer_back_right.transform.rotation;
        startOrientation[12] = center.transform.rotation;
    }

    public void moveToStart() {
        print("resetting");
        center_front_left.transform.position = startPositions[0];
        center_front_right.transform.position = startPositions[1];
        center_back_left.transform.position = startPositions[2];
        center_back_right.transform.position = startPositions[3];
        middle_front_left.transform.position = startPositions[4];
        middle_front_right.transform.position = startPositions[5];
        middle_back_left.transform.position = startPositions[6];
        middle_back_right.transform.position = startPositions[7];
        outer_front_left.transform.position = startPositions[8];
        outer_front_right.transform.position = startPositions[9];
        outer_back_left.transform.position = startPositions[10];
        outer_back_right.transform.position = startPositions[11];
        center.transform.position = startPositions[12];

        for(var i = 0; i < lastPositions.Length; i++) {
            lastPositions[i] = startPositions[i];
        }
        
        center_front_left.transform.rotation = startOrientation[0];
        center_front_right.transform.rotation = startOrientation[1];
        center_back_left.transform.rotation = startOrientation[2];
        center_back_right.transform.rotation = startOrientation[3];
        middle_front_left.transform.rotation = startOrientation[4];
        middle_front_right.transform.rotation = startOrientation[5];
        middle_back_left.transform.rotation = startOrientation[6];
        middle_back_right.transform.rotation = startOrientation[7];
        outer_front_left.transform.rotation = startOrientation[8];
        outer_front_right.transform.rotation = startOrientation[9];
        outer_back_left.transform.rotation = startOrientation[10];
        outer_back_right.transform.rotation = startOrientation[11];
        center.transform.rotation = startOrientation[12];
        
        center_front_left.reset();
        center_front_right.reset();
        center_back_left.reset();
        center_back_right.reset();
        middle_front_left.reset();
        middle_front_right.reset();
        middle_back_left.reset();
        middle_back_right.reset();
        outer_front_left.reset();
        outer_front_right.reset();
        outer_back_left.reset();
        outer_back_right.reset();
        
        initialX = center.position.x;
        lastProg = 0;
    }
}