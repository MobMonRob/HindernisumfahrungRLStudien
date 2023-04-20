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
    private Vector3 lastCenterPosition = Vector3.zero;
    private Quaternion[] startOrientation = new Quaternion[13];

    private void Start() {
        storeStart();
        lastCenterPosition = center.position;

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
    public float getProgress(Vector3 targetPosition) {
        var previousDistanceToTarget = Vector3.Distance(clearY(lastCenterPosition), clearY(targetPosition));
        var currentDistanceToTarget = Vector3.Distance(clearY(getCenterPosition()), clearY(targetPosition));
        var progress = previousDistanceToTarget - currentDistanceToTarget;
        
        return progress;
    }

    public Vector3 getCenterPosition() {
        return center.position;
    }

    public Vector3 clearY(Vector3 vec) {
        vec.y = 0;
        return vec;
    }

    public float getReward(Vector3 targetPosition) {
        if (isFlipped()) {
            return -10.0f;
        }

        var punishment = -0.0025f;
        punishment += getAngle() * -0.0025f;

        var progress = getProgress(targetPosition);
        lastCenterPosition = getCenterPosition();
        return progress + punishment;
    }

    public bool isFlipped() {
        var up = center.forward;
        var angle = Vector3.Angle(up, Vector3.up);
        return angle > 90;
        // return angle > 3;
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
        
        lastCenterPosition = center.position;
    }
}