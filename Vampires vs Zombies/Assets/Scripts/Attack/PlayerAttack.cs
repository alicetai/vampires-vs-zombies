using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

[Serializable]
public class PlayerAttack : DefaultAttack, IAttackable {

    
    private GameObject camera;
    public PlayerAttack(Character c, GameObject camera, float damage) : base(c, damage) {
        this.camera = camera;
        
    }

    public override AttackOutcome attack() {
        
        AttackOutcome outcome = base.attack();
        
        if (outcome == AttackOutcome.Infected) {

            // Zoom out camera if player is biting

            camera.GetComponent<MouseVisionController>().Status = CharacterStatus.Paralysed;
            camera.transform.localPosition +=  new Vector3(0, 0.62f, 0);
            //camera.transform.localPosition = -(1.5f * camera.transform.forward) + new Vector3(0, 0.8f, 0);

            Manager.Instance.ExecuteAfter(() => {
                c.Status = CharacterStatus.Active;
                // Reset camera
                camera.GetComponent<MouseVisionController>().Status = CharacterStatus.Active;
                camera.transform.localPosition = new Vector3(0,0.5f,0.2f); // reattach character
                
            }, 3);

        }else if (outcome != AttackOutcome.Missed){
            // Only play animation and blood splatter if NOT missed
            
            animator.SetTrigger("Attacks");

            AttackingSound();

            // Trigger particle system blood splatter immediately
            GameObject blood = GameObject.Instantiate(bloodMist);
            blood.transform.position = c.transform.position + (1.5f * c.transform.forward);
        }

        return outcome;
    }


}