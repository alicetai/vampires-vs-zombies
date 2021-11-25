using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum CharacterType {
    Villager = 0,
    Vampire = 1,

    Zombie = 2
}

public enum CharacterStatus {
    Active,
    Paralysed,
    Stunned,
    Dead
}

public enum Difficulty {
    Easy,
    Hard
}


public class Character : MonoBehaviour, ICharacterLike
{

    // public Character(CharacterType type) {
    //     this.CharacterType = type;
    //     this.Status = CharacterStatus.Active;
    // }

    // Character controller
    public CharacterController controller;
    
    // Character variables
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // Health bar
    public HealthBarController healthBar;

    // Variables for attacking behavior
    public Transform attackPoint;
    public float attackRange;

    // Variables for special abilities
    public float leapRange = 3f;
    public float stunRange = 5f;

    // Move behaviour
    public IMovable moveBehaviour;
    public void move(Vector3 dir) {moveBehaviour.move(dir);}
    public void leap(){moveBehaviour.leap();}
    
    // Control behaviour (AI or Player)
    public IControllable controlBehaviour;
    public void act() {

        // If this is paralysed don't act
        if (this.Status == CharacterStatus.Paralysed || this.Status == CharacterStatus.Stunned) {
            return;
        }
        controlBehaviour.act();
    }

    public void setTarget(Character target) {controlBehaviour.setTarget(target);}
    
    // Attack behaviour
    public IAttackable attackBehaviour;
    public AttackOutcome attack() {return attackBehaviour.attack();}
    public void stun() {attackBehaviour.stun();}
    
    // Damage behaviour
    public IDamageable damageBehaviour;
    public AttackOutcome registerHit(Character attackingEntity, float damageDealt) { return damageBehaviour.registerHit(attackingEntity, damageDealt);}
    public void healHealth(float health){damageBehaviour.healHealth(health);}
    public float GetHealthPercentage() {return damageBehaviour.GetHealthPercentage();}

    // Animator for changing controller
    Animator animator; 
    
    // Make attack range visible in editor
    void OnDrawGizmosSelected(){
        if (attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, stunRange);
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);  
    }

    // Status and an associated time (for paralysis time in seconds from now)
    private CharacterStatus _status;
    public CharacterStatus Status { get; set; }

    // Set character type and associated behaviours
    private CharacterType _characterType;
    public CharacterType CharacterType {
        get => _characterType;
        set {
            _characterType = value;
            switch (value) {
                case CharacterType.Villager:
                    controlBehaviour = new VillagerAiController(this); 

                    // Set animator controller
                    animator = this.GetComponentInChildren<Animator>();
                    if(animator.gameObject.activeSelf) {
                        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Controllers/Villager_Animator");
                    }

                    if (PlayerVariables.PlayerDifficulty == Difficulty.Hard){
                        // Hard difficulty  
                        moveBehaviour = new AiMove(this, 5.0f);
                        damageBehaviour = new DefaultDamage(this, 6.0f); // Starting Health

                    }else{
                        // Default easy difficulty
                        moveBehaviour = new AiMove(this, 4.0f); 
                        damageBehaviour = new DefaultDamage(this, 3.0f); // Starting Health
                    }
                    
                    break;

                case CharacterType.Vampire:
                    controlBehaviour = new VampireAiController(this);

                    // Set animator controller
                    animator = this.GetComponentInChildren<Animator>();
                    
                    if(animator.gameObject.activeSelf) {
                        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Controllers/Vampire_Animator");
                    }

                    // Set new health bar colour
                    healthBar.SetColour(Color.cyan);

                    // Set 'stats' based on difficulty 
                    if (PlayerVariables.PlayerDifficulty == Difficulty.Hard){
                        // Hard difficulty  
                        attackBehaviour = new AiAttack(this, 1.0f);
                        moveBehaviour = new AiMove(this, 6.0f);
                        damageBehaviour = new DefaultDamage(this, 14.0f); // Higher Starting health

                    }else{
                        // Default easy difficulty
                        attackBehaviour = new AiAttack(this, 0.5f);
                        moveBehaviour = new AiMove(this, 5.0f); // Faster than zombie
                        damageBehaviour = new DefaultDamage(this, 8.0f); // Higher Starting health
                    }

                    break;

                
                case CharacterType.Zombie:
                    // Set animator controller
                    animator = this.GetComponentInChildren<Animator>();
                    if(animator.gameObject.activeSelf) {
                        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Controllers/Zombie_Animator");
                    }

                    controlBehaviour = new ZombieAiController(this);

                    // Set health bar colour
                    healthBar.SetColour(Color.green);

                    // Set 'stats' based on difficulty 
                    if (PlayerVariables.PlayerDifficulty == Difficulty.Hard){
                        // Hard difficulty  
                        attackBehaviour = new AiAttack(this, 1.3f);
                        moveBehaviour = new AiMove(this, 5.3f);
                        damageBehaviour = new DefaultDamage(this, 12.0f); // Starting health

                    }else{
                        // Default easy difficulty
                        attackBehaviour = new AiAttack(this, 0.8f); // Hits harder than vampires
                        moveBehaviour = new AiMove(this, 4.3f);
                        damageBehaviour = new DefaultDamage(this, 6.0f); // Starting Health
                    }

                    break;
                

                default: 
                    moveBehaviour = new DefaultMove(3.0f);
                    break;
                    
            }
        }
    }

}
