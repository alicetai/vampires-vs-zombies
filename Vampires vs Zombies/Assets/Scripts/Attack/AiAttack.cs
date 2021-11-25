using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

[Serializable]
public class AiAttack : DefaultAttack, IAttackable {

    
    private GameObject camera;
    public AiAttack(Character c, float damage) : base(c, damage) {

    }

    public override AttackOutcome attack() {
        
        AttackOutcome outcome = base.attack();
        
        if (outcome == AttackOutcome.Dead) {
            // If target dies
            animator.SetTrigger("Attacks");

            // Give animation a moment to swipe
            Manager.Instance.ExecuteAfter(() => {
                // Trigger particle system blood splatter
                GameObject blood = GameObject.Instantiate(bloodMist);
                blood.transform.position = c.transform.position + (1.5f * c.transform.forward);

                // Trigger attacking sound
                AttackingSound();
            }, 1);

            // Make character wait while target dies
            c.Status = CharacterStatus.Paralysed;

            Manager.Instance.ExecuteAfter(() => {
                c.Status = CharacterStatus.Active;
            }, 4);
            

        }else if (outcome == AttackOutcome.Survived){

            animator.SetTrigger("Attacks"); // Target doesn't die - we attack

            // Give animation a moment to swipe
            Manager.Instance.ExecuteAfter(() => {
                // Trigger particle system blood splatter
                GameObject blood = GameObject.Instantiate(bloodMist);
                blood.transform.position = c.transform.position + (1.5f * c.transform.forward);

                // Trigger attacking sound
                AttackingSound();
            }, 1);
        }

        return outcome;
    }


}