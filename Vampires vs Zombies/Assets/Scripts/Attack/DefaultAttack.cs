using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

[Serializable]
public class DefaultAttack : IAttackable
{
    protected GameObject bloodMist;
    public float damageDealt; // Amount of damage dealt by this attack
    
    protected Animator animator;
    protected Animator targetAnimator;

    AudioSource attackingSoundSource;
    AudioClip[] attackingSounds; // list of attacking sounds (there's more than one)
    AudioClip bitingSound; // biting sound for zombies
    AudioClip suckingSound; // sucking sound for vampires

    protected Character c;

    public DefaultAttack(Character c) {
        //infected = (Material)Resources.Load("infectedWhite", typeof(Material));
        bloodMist = (GameObject)Resources.Load("Particle System/BloodMist", typeof(GameObject));
        LoadAudio();
        this.c = c;
    }

    // set damage dealt
    public DefaultAttack(Character c, float damage) {
        this.damageDealt = damage;
        bloodMist = (GameObject)Resources.Load("Particle System/BloodMist", typeof(GameObject));
        LoadAudio();
        this.c = c;
    }

    public virtual AttackOutcome attack(){

        // Get animator if not previously done
        if (animator == null) {
            animator = c.GetComponentInChildren<Animator>();
        }


        // Code below adapted from "MELEE COMBAT in Unity" tutorial, by Brackeys https://www.youtube.com/watch?v=sPiVz1k-fEs

        // Detect enemies in range of attack 
        Collider[] targetsHit = Physics.OverlapSphere(c.attackPoint.position, c.attackRange);

        // Iterate through targets hit and cause damage
        foreach(Collider target in targetsHit) {
            
            // End of adapted code

            // Get target character
            Character targetCharacter = target.GetComponent<Character>();


            // Skip entity if no attached character
            if (!targetCharacter)
                continue;

            // If target character does not equal type of attacking character e.g. if player is vampire it doesn't deal damage to vampires
            // If target character is NOT dead
            if (targetCharacter && targetCharacter.CharacterType != c.CharacterType && targetCharacter.Status != CharacterStatus.Dead) {

                AttackOutcome outcome = targetCharacter.registerHit(c, damageDealt);

                // Infect target
                if (outcome == AttackOutcome.Infected) { 

                    animator.SetTrigger("Bites"); // Target dies/gets infected - we bite

                    // Make character "jump" to get to the target 
                    //c.transform.LookAt(target.transform.position);
                    c.transform.position = target.transform.position - 0.35f * (target.transform.position - c.transform.position).normalized;
                    
                    c.Status = CharacterStatus.Paralysed; // make character wait while biting

                    // trigger biting sounds
                    switch (c.CharacterType)
                    {
                        case (CharacterType.Vampire):
                            SuckingSound();
                            break;
                        case (CharacterType.Zombie):
                            BitingSound();
                            break;
                        default:
                            break;
                    }
    
                    Manager.Instance.ExecuteAfter(() => {
                        // Reactivate character
                        c.Status = CharacterStatus.Active;
                    }, 3);
                }
                return outcome;
                  
            }
        }

        // The loop has finished and no target was hit
        animator.SetTrigger("Attacks"); //just play attack animation

        return AttackOutcome.Missed;
    }


    // Special ability stun for zombies
    public virtual void stun(){

        //Debug.Log("Stun all characters in " + c.stunRange + " range");

        // Detect characters in range
        Collider[] targetsStunned = Physics.OverlapSphere(c.attackPoint.position, c.stunRange);

        // Iterate through targets hit and stun them
        foreach(Collider target in targetsStunned) {
            
            // Get target character
            Character targetCharacter = target.GetComponent<Character>();
            
            // Skip entity if no attached character
            if (!targetCharacter)
                continue;

            // If target character does not equal type of attacking character e.g. if player is vampire it doesn't deal damage to vampires
            // If target character is NOT dead
            //if (targetCharacter && targetCharacter.CharacterType != c.CharacterType && targetCharacter.Status != CharacterStatus.Dead) {
            if (targetCharacter && targetCharacter.CharacterType != CharacterType.Zombie && targetCharacter.Status != CharacterStatus.Dead) {    
                // Stun targets 
                //Debug.Log("Stuns" + targetCharacter.name);
                targetCharacter.Status = CharacterStatus.Stunned;

                // TRIGGER STUN ANIMATION
                targetAnimator = targetCharacter.GetComponentInChildren<Animator>();
                targetAnimator.SetBool("IsStunned", true);
                targetAnimator.Play("GetsBitten");

                

                // change health bar color to indicate stunned status
                targetCharacter.healthBar.setStunned(Color.yellow); 


                // Reactivate stunned character after 5 seconds
                Manager.Instance.ExecuteAfter(() => {

                    // Reactivate character if character isn't already paralysed
                    if (targetCharacter.Status != CharacterStatus.Paralysed){
                        targetCharacter.Status = CharacterStatus.Active;
                        // REACTIVATE ANIMATION
                        targetAnimator = targetCharacter.GetComponentInChildren<Animator>();
                        targetAnimator.SetBool("IsStunned", false);
                    }
                    
                    // Change health bar colour back
                    targetCharacter.healthBar.SetColour(targetCharacter.healthBar.GetColour());

                    //Debug.Log("Reactivate " + targetCharacter.name);
                    
                }, 5);
            }
        }
    }

    void LoadAudio()
    {
        attackingSounds = Array.ConvertAll(Resources.LoadAll("Sound/Attack", typeof(AudioClip)), asset => (AudioClip)asset);
        bitingSound = (AudioClip)Resources.Load("Sound/ZombieBiting", typeof(AudioClip));
        suckingSound = (AudioClip)Resources.Load("Sound/VampireSucking", typeof(AudioClip));
    }

    public void AttackingSound()
    {
        // get the source of the sound
        attackingSoundSource = c.gameObject.GetComponent<AudioSource>();

        // randomly pick an attacking sound from the list to play
        AudioClip randomAttackingSound = attackingSounds[UnityEngine.Random.Range(0, attackingSounds.Length)];
        attackingSoundSource.clip = randomAttackingSound;
        // Set volume
        if (PlayerVariables.gameVolume == 0f){
            attackingSoundSource.volume = 0f;
        }else{
            attackingSoundSource.volume *= (PlayerVariables.gameVolume + 0.3f);
        }

        attackingSoundSource.PlayOneShot(randomAttackingSound);
    }

    public void BitingSound()
    {
        // get the source of the sound
        attackingSoundSource = c.gameObject.GetComponent<AudioSource>();

        // assign the biting sound as the audio clip
        attackingSoundSource.clip = bitingSound;

        // Set volume
        if (PlayerVariables.gameVolume == 0f){
            attackingSoundSource.volume = 0f;
        }else{
            attackingSoundSource.volume *= (PlayerVariables.gameVolume + 0.3f);
        }


        attackingSoundSource.PlayOneShot(bitingSound);
    }

    public void SuckingSound()
    {
        // get the source of the sound
        attackingSoundSource = c.gameObject.GetComponent<AudioSource>();

        // assign the sucking sound as the audio clip
        attackingSoundSource.clip = suckingSound;

        // Set volume
        if (PlayerVariables.gameVolume == 0f){
            attackingSoundSource.volume = 0f;
        }else{
            attackingSoundSource.volume *= (PlayerVariables.gameVolume + 0.3f);
        }
        
        attackingSoundSource.PlayOneShot(suckingSound);
    }

    // Get the damage of the object
    // public int GetDamage()
    // {
    //     return this.damageDealt;
    // }

}
