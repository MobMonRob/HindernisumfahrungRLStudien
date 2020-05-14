using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using UnityEngine;

public class ComPortMirror : MonoBehaviour {
    private static readonly int useRange = 40;

    public int COMPORT = 6;
    public int updatedPerSec = 8;
    public SpiderController controller;
    private SerialPort stream;
    private float timePassed;

    // Start is called before the first frame update
    void Start() {
        stream = new SerialPort("COM" + COMPORT, 115200);
        stream.ReadTimeout = 50;
        stream.Open();
    }

    // Update is called once per frame
    void FixedUpdate() {

        var updateIntervall = 1f / updatedPerSec;

        timePassed += Time.deltaTime;
        if (timePassed > updateIntervall) {
            timePassed -= updateIntervall;
            sendData();
        }
    }

    private void sendData() {
        var servos = controller.allServos;
        //pack all 12 servo angles into one int
        string superMessage = "";
        for (int i = 0; i < 12; i++) {
            int angle = (int) servos[i].currentAngle;
            angle += 60;        //script used -60 -> + 60, arduino used 0-120
            int angleClamped = Clamp(angle, 60 - useRange, 60 + useRange);
            superMessage += angleClamped + ";";
        }
        print(superMessage);
        writeToArduino(superMessage);
    }

    private void writeToArduino(string message) {
        stream.WriteLine(message);
        stream.BaseStream.Flush();
    }

    private static int Clamp(int value, int min, int max) {
        return (value < min) ? min : (value > max) ? max : value;
    }

    private void OnApplicationQuit() {
        for (int i = 0; i < 12; i++) {
            controller.allServos[i].currentAngle = 0;
            controller.allServos[i].targetAngle = 0;
        }
        
        sendData();
        
        stream.Close();
    }
}