using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable {
    
    AttackOutcome attack();

    void stun();

}
