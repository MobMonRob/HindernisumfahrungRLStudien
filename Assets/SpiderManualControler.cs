using System;
using UnityEngine;

public class SpiderManualControler : MonoBehaviour {

    public SpiderController controller;
    
    private float timePassed = 0f;
    private int counter = 0;
    public int resetAt = 1500;

    private static float[] values;

    private void Start() {
        values = new float[12];
        for (int i = 0; i < 12; i++) {
            values[i] = 0;
        }
    }

    private void FixedUpdate() {
        timePassed += Time.deltaTime;
        counter++;

        if (counter % resetAt == 0) {
            controller.moveToStart();
            return;
        }

        var x_scale = 5f;
        var y_scale = 25f;

        values[0] = getSin(x_scale, 0, y_scale, timePassed);
        values[1] = getSin(x_scale, 0.25f, y_scale, timePassed);
        values[2] = getSin(x_scale, 0.5f, y_scale, timePassed);
        values[3] = getSin(x_scale, 0.75f, y_scale, timePassed);

        var center_offset = 0.5f;

        values[4] = getSin(x_scale, 0 + center_offset, y_scale, timePassed);
        values[5] = getSin(x_scale, 0.25f + center_offset, y_scale, timePassed);
        values[6] = getSin(x_scale, 0.5f + center_offset, y_scale, timePassed);
        values[7] = getSin(x_scale, 0.75f + center_offset, y_scale, timePassed);
        
    }

    public static float[] getValues() {
        return values;
    }
    
    //offset is in % to sin
    private float getSin(float x_scale, float offset, float y_scale, float time) {
        return (float) (Math.Sin(x_scale * time + offset * 2 * Math.PI) * y_scale);
    }

    
}