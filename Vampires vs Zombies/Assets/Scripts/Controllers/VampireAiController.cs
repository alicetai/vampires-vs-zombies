using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VampireAiController : BaseAiController {

    
    //private Character target;

    
    public VampireAiController(Character c) : base(c) {
        trackingDist = 30f;
        
    }

    public override Character findTarget (Character c, CharacterType typeToAvoid) {
        float nearest = float.MaxValue;
        Character target = null;

        foreach (Character pTarget in Manager.Instance.characters) {
            //Debug.Log(pTarget);
            if (pTarget.CharacterType == typeToAvoid || pTarget.Status == CharacterStatus.Paralysed)
                continue;
            float dist = (pTarget.transform.position - c.transform.position).magnitude;
            if (dist < trackingDist && dist < nearest) {
                nearest = dist;
                target = pTarget;
            }
        }
        return target;

    }

    public override void act() {
        //Debug.Log(c.CharacterType);
        
        if (target && (target.CharacterType == CharacterType.Vampire || target.Status == CharacterStatus.Paralysed || target.Status == CharacterStatus.Dead))
            target = null;

        if (Time.frameCount % 60 == 0 && (!target)) {
            //target = findTarget(c, CharacterType.Villager) ?? findTarget(c, CharacterType.Zombie);
            target = findTarget(c, CharacterType.Vampire); // prioritises villagers and enemies equally
        }


        
        if (target) {
            Vector3 toTarget = target.transform.position - c.transform.position;

            if (toTarget.magnitude <= trackingDist) {
                // Rotate character to look at target
                c.transform.LookAt(target.transform.position);

                // Move towards target
                Vector3 dir = new Vector3(toTarget.x, 0, toTarget.z).normalized;
                c.move(dir);
                
                if (toTarget.magnitude <= c.attackRange + 2f) {

                    // Attack
                    if (Time.frameCount % 30 == 0){
                        c.attack();
                    }

                }
                return;
            }
        }

        base.act();

    }


}
