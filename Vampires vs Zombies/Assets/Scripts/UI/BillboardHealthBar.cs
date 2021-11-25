using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardHealthBar : MonoBehaviour
{
    public Transform cameraTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject camera = GameObject.Find("Main Camera");
        cameraTransform = camera.transform;
    }

    // Code below adapted from "How to make a HEALTH BAR in Unity!" tutorial, by Brackeys https://www.youtube.com/watch?v=BLfNP4Sc_iA

    // Called after regular Update() function
    void LateUpdate()
    {
        // Make sure health bar always points towards camera
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}
