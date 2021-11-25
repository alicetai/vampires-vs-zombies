using UnityEngine;
using System.Collections;


public class BaseAiController : IControllable {

    private Vector3 direction = (new Vector3 (Random.value - 0.5f, 0, Random.value - 0.5f)).normalized * 0.4f;
    protected float trackingDist = 30f;

    protected Character c;
    protected int tick;

    protected Character target;


    public BaseAiController(Character c) {
        this.c = c;
    }

    public virtual void act() {
        if (tick % 240 == 0) 
            direction = (new Vector3 (Random.value - 0.5f, 0, Random.value - 0.5f)).normalized * 0.4f;
            //direction = direction.normalized;
        
        
        // Move
        c.move(direction);
        tick++;

    }

    public virtual void setTarget(Character target){
        this.target = target;
    }

    public virtual Character findTarget (Character c, CharacterType targetType) {
        float nearest = float.MaxValue;
        Character target = null;

        foreach (Character pTarget in Manager.Instance.characters) {
            //Debug.Log(pTarget);
            if (pTarget.CharacterType != targetType || pTarget.Status == CharacterStatus.Paralysed)
                continue;
            float dist = (pTarget.transform.position - c.transform.position).magnitude;
            if (dist < trackingDist && dist < nearest) {
                nearest = dist;
                target = pTarget;
            }
        }
        return target;

    }
    
}
