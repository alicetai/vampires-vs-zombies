using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class DefaultDamage : IDamageable
{   

    // Variables for damage system and health
    // Can change depending on type of damage / type of character
    public float startHealth;
    private float currentHealth;

    private Animator animator;

    private Mutate mutate;

    protected Character c;

    public DefaultDamage(Character c){
        this.startHealth = 10.0f; // Default health is 10
        this.ResetHealth();
        this.c = c;
    }

    // Initialise with starting health
    public DefaultDamage(Character c, float startHealth){
        this.startHealth = startHealth;
        this.c = c;
        this.ResetHealth();
    }


    // Reset health to starting health
    public void ResetHealth()
    {
        currentHealth = startHealth;
    }

    // Regenerate this character's health by indicated amount
    public void healHealth(float health){
        if ((this.currentHealth + health) <= startHealth){
            this.currentHealth += health;
        }else{
            ResetHealth(); // set to max healh
        }
    }


    public AttackOutcome registerHit(Character attackingEntity, float damageDealt)
    {
        // Get animator if not previously done
        if (animator == null) {
            animator = c.GetComponentInChildren<Animator>();
            //Debug.Log("animator loaded");
        }

        if (mutate == null){
            mutate = c.GetComponentInChildren<Mutate>();
        }

        //Debug.Log(c.CharacterType + " In DefaultDamage, register hit by: " + attackingEntity.CharacterType);
        
        // Decrease current object's health by damage
        currentHealth -= damageDealt;
        //Debug.Log("Target damaged, current health: "+ currentHealth);

        // Set character's healthbar 
        c.healthBar.SetHealth(GetHealthPercentage());


        // If they survive
        if (currentHealth > 0) {
        
            // Gets attacked animation 
            animator.SetTrigger("GetsAttacked");

            // If a zombie or vampire gets attacked
            if (c.CharacterType == CharacterType.Zombie || c.CharacterType == CharacterType.Vampire){
                // change its target to the attacking entity
                c.setTarget(attackingEntity);
            }

            return AttackOutcome.Survived;

        }
        
        // If villager
        if (c.CharacterType == CharacterType.Villager) {

            // Infect villager 
            animator.SetTrigger("GetsBitten");
            animator.SetBool("IsStunned", true);


            // Trigger mutate script
            mutate.MutateInto(attackingEntity.CharacterType, 3); // Color-changing animation for infection

            // make villager wait while biting
            c.Status = CharacterStatus.Paralysed;
            c.transform.Find("HealthBarCanvas").gameObject.SetActive(false); // Temporarily disable heath bar for target

            Manager.Instance.ExecuteAfter(() => {
                // Reenable health bar
                c.transform.Find("HealthBarCanvas").gameObject.SetActive(true);
                // Change character type of villager
                c.CharacterType = attackingEntity.CharacterType;
                c.Status = CharacterStatus.Active;
            }, 3);
            

            // Reset health bar to full
            c.healthBar.SetMaxHealth();
            this.ResetHealth(); // reset health variable
        
            //Debug.Log("Villager infected! " + c.CharacterType + " Reset health to : " + currentHealth);

            return AttackOutcome.Infected;
        }

        // If health is zero AND is a zombie or vampire

        // Die animation
        animator.SetBool("IsDead", true);
        animator.SetTrigger("Dies");

        // Set character to dead, will remove from character list
        c.Status = CharacterStatus.Dead;

        // Disable controller and collider
        c.GetComponent<CharacterController>().enabled = false;
        c.GetComponent<CapsuleCollider>().enabled = false;

        // Disable health bar
        c.transform.Find("HealthBarCanvas").gameObject.SetActive(false);

        return AttackOutcome.Dead;
        
    }

    public float GetHealthPercentage(){
        return(currentHealth/startHealth);
    }

}
