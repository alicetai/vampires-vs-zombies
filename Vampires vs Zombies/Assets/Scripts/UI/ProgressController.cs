using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProgressController : MonoBehaviour
{
    public Slider progress;
    
    void Start()
    {
        Reset(); // Reset progress bar at start
    }

    public void Reset(){
        progress.value = 0f;
    }

    public void SetProgress(float progressPercentage){
        progress.value = progressPercentage; // round up to 1 decimal place
    }
}
