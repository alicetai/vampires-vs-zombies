using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VillagerAiController : BaseAiController {


    private Animator animator;
    
    //private Character target;

    public VillagerAiController(Character c) : base(c) {
        trackingDist = 10f;

    }

    
    public override void act() {
        //Debug.Log(c.CharacterType);
        

        // Get animator if not previously done
        if (animator == null) {
            animator = c.GetComponentInChildren<Animator>();
            //Debug.Log("animator loaded");
        }

        if (Time.frameCount % 20 == 0)
            target = findTarget(c, CharacterType.Vampire) ?? findTarget(c, CharacterType.Zombie);
        if (target) {
            

            // Run away from target
            Vector3 fromTarget = c.transform.position - target.transform.position;
            
            if (fromTarget.magnitude <= trackingDist) {
                // Character runs away from target
                c.transform.LookAt(c.transform.position - target.transform.position);

                // Run awayyyyy
                animator.SetBool("IsThreatened", true);
                Vector3 dir = new Vector3(fromTarget.x, 0, fromTarget.z).normalized;
                tick = 0;
                c.move(dir);
                
                
                return;
            }
        }

        animator.SetBool("IsThreatened", false);
        base.act();
        


    }

}
