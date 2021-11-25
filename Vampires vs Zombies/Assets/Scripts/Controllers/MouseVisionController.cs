using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseVisionController : MonoBehaviour
{
    
    //public float mouseSensitivity = 50f;
    public Transform player; 
    float xRotation = 0f;
    //float yRotation = 0f;
    public float maxZoom = 2f;

    // Status and an associated time (for paralysis time in seconds from now)
    private CharacterStatus _status;
    public CharacterStatus Status { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        // hide and lock cursor to screen - i.e. it can't leave the window 
        //Cursor.lockState = CursorLockMode.Locked;
        this.Status = CharacterStatus.Active;
    }

    // Update is called once per frame
    void Update()
    {   
        if (this.Status != CharacterStatus.Paralysed){

            //Debug.Log("Camera status:" + this.Status);


            // Code below adapted from "FIRST PERSON MOVEMENT in Unity - FPS Controller tutorial" by Brackeys https://www.youtube.com/watch?v=_QajrabyTJc

            float mouseX = Input.GetAxis("Mouse X") * PlayerVariables.mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * PlayerVariables.mouseSensitivity * Time.deltaTime;

            // rotate player's body to 'look' left and right
            player.Rotate(Vector3.up * mouseX);

            // Increase xRotation every frame based on mouseY
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 45f); // clamp rotation values
            // negative : upwards ; positive : downwards

            // rotate camera up and down (along x axis)
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // End of adapted code

        // Camera pauses when player is biting
        }else if(this.Status == CharacterStatus.Paralysed){
            
            // Rotate camera 90 degrees so that it is pointing down
            GetComponent<Camera>().transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            // Zoom out 
            if (GetComponent<Camera>().transform.localPosition.y < maxZoom){
                GetComponent<Camera>().transform.localPosition +=  new Vector3(0, 0.02f, 0);
            }
        }

    }

}
