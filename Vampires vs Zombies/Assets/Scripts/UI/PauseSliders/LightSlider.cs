using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// How To Make A Volume Slider In 4 Minutes - Easy Unity Tutorial by Hooson https://www.youtube.com/watch?v=yWCHaTwVblk&list=PLZb-pbd1-DXm7xq5cM9wysZEsf8VNzOTO&index=16&ab_channel=Hooson  


public class LightSlider : MonoBehaviour
{
    public Slider lightSlider;
    public Light gameLight;

    // Start is called before the first frame update
    void Start()
    {
        if (lightSlider != null) {
            setLight();
        }
        
    }

    public void setLight() {
        gameLight.intensity =  lightSlider.value;
    }

    
}
