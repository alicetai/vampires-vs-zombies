using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// How To Make A Volume Slider In 4 Minutes - Easy Unity Tutorial by Hooson https://www.youtube.com/watch?v=yWCHaTwVblk&list=PLZb-pbd1-DXm7xq5cM9wysZEsf8VNzOTO&index=16&ab_channel=Hooson  

public class MouseSlider : MonoBehaviour
{
    public Slider mouseBar;
    
    void Start()
    {
        if (mouseBar != null){
            SetSensitivity();
        }
    }

    // sets value based on slider 
    public void SetSensitivity() {
        PlayerVariables.mouseSensitivity = mouseBar.value;
    }
}
