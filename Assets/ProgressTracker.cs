﻿using System.Collections;
 using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ProgressTracker : MonoBehaviour {

     public SpiderController controller;
     private Text text;

     // Start is called before the first frame update
     void Start() {
         text = this.GetComponent<Text>();
     }
 
     // Update is called once per frame
     void Update() {

        //  text.text = controller.getProgress().ToString(CultureInfo.CurrentCulture);

     }
 }