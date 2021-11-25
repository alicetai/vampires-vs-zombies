using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    //public Image healthBar;
    public Slider healthBar;
    public Image healthBarImage;
    private Color healthBarColour;
    
    // Code below adapted from "How to make a HEALTH BAR in Unity!" tutorial, by Brackeys https://www.youtube.com/watch?v=BLfNP4Sc_iA
    void Start()
    {
        SetMaxHealth(); // set to max at start
        this.healthBarColour = healthBarImage.color;
    }

    public void SetMaxHealth(){
        //healthBar.fillAmount = 1.0f;
        healthBar.value = 1.0f;
    }

    public void SetHealth(float healthPercentage){
        //healthBar.fillAmount = healthPercentage;
        healthBar.value = healthPercentage;
    }

    // End of adapted code
    
    public void SetColour(Color colour){
        healthBarImage.color = colour;
        this.healthBarColour = colour;
    }

    public void setStunned(Color colour){
        healthBarImage.color = colour;
    }

    public Color GetColour(){
        return this.healthBarColour;
    }

}
