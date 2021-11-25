using UnityEngine;
using System;
public class AiMove : DefaultMove {
    
    // AiMove constructor
    public AiMove(Character c, float speed) : base(c, speed) {
    }


    public override void move(Vector3 dir) { 
        // Check if character is touching the ground
        isGrounded = Physics.CheckSphere(c.groundCheck.position, c.groundDistance, c.groundMask);

        if (isGrounded && velocity.y < 0){
            velocity.y = -5f;
        }

        // Move character based on which direction they are facing
        Vector3 move = new Vector3(dir.x, 0, dir.z);
        
        move = move.magnitude > 1 ? move.normalized : move;
        //Debug.Log(move.magnitude);
        // Rotate character
        c.transform.LookAt(c.transform.position + move);
        //c.transform.forward = c.transform.right * dir.x  + c.transform.forward * dir.z;

        // Move character
        c.controller.Move(move * speed * Time.deltaTime);

        // Implement gavity
        velocity.y += gravity * Time.deltaTime;
        c.controller.Move(velocity * Time.deltaTime);
        
    }

}