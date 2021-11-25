using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackOutcome {
    Survived,
    Dead,
    Infected,
    Missed
}

public interface IDamageable {
    
    AttackOutcome registerHit(Character attackingEntity, float damageDealt);

    void healHealth(float health);

    float GetHealthPercentage();

}
