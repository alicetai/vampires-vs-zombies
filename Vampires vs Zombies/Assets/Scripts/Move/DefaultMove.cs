using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DefaultMove : IMovable
{
    
    protected Vector3 velocity;
    protected bool isGrounded;

    public float speed; // Default speed sensitivity
    public float gravity = -9.8f;

    //float horizontalMove = 0f;

    protected Character c;
    // Default constructor 
    public DefaultMove(Character c, float speed){
        // default speed
        this.speed = speed;
        this.c = c;
        
    }

    // Constructor which sets speed
    public DefaultMove(float speed){
        this.speed = speed;
    }

    // Code below adapted from "FIRST PERSON MOVEMENT in Unity - FPS Controller" tutorial by Brackeys https://www.youtube.com/watch?v=_QajrabyTJc
    
    // Move controller in a specified direction
    public virtual void move(Vector3 dir) {

        // Check if character is touching the ground
        isGrounded = Physics.CheckSphere(c.groundCheck.position, c.groundDistance, c.groundMask);

        if (isGrounded && velocity.y < 0){
            velocity.y = -5f;
        }

        // Move character based on which direction they are facing
        Vector3 move = c.transform.right * dir.x  + c.transform.forward * dir.z;


        // Rotate character to look towards new direction
        //c.transform.LookAt(move);
        //c.transform.forward = move;

        move = move.magnitude > 1 ? move.normalized : move;
        // Move character
        c.controller.Move(move * speed * Time.deltaTime);

        //Update speed for animator
        //horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        //animator.SetFloat("Speed", horizontalMove);

        // Implement gavity
        velocity.y += gravity * Time.deltaTime;
        c.controller.Move(velocity * Time.deltaTime);


        // Draw a ray pointing at our target
        //Debug.DrawRay(c.transform.position, c.transform.forward, Color.blue);
        //Debug.DrawRay(c.transform.position,  move, Color.yellow);
    }


    // End of adapted code

    public virtual void leap(){
        // leap forward

        //Debug.Log("Leap! from " + c.transform.position);

        c.controller.Move(c.transform.forward * c.leapRange);
            
        
        
    }
    
}


