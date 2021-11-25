using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : IControllable {

    private Character c;
    public float maxCPS = 2f; // Max clicks per second
    float clickTimeout = 0;
    float spaceBarTimeout = 10f; // start with max

    float healthRegenerateTimer = 0; 
    float healthRegenerateTimeout = 3f; // health regenerates if player stands still for three seconds

    float stunCoolDown = 10f;
    float leapCoolDown = 2f;

    Image crosshair;


    public PlayerController(Character c) {
        this.c = c;
        this.crosshair = GameObject.Find("Cross").GetComponent<Image>(); // Load cross hair
    }

    public void act() {

        // Code below adapted from "FIRST PERSON MOVEMENT in Unity - FPS Controller tutorial" by Brackeys https://www.youtube.com/watch?v=_QajrabyTJc

        // WASD Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        c.move(new Vector3(x, 0, z));

        // End of adapted code


        // Detect enemies/villagers in range
        Collider[] targetsInRange = Physics.OverlapSphere(c.attackPoint.position, c.attackRange);

        bool inRange = false;
        
        // Iterate through targets and check
        foreach(Collider target in targetsInRange) {
            
            // Get target character
            Character targetCharacter = target.GetComponent<Character>();
            
            // Skip entity if no attached character
            if (!targetCharacter)
                continue;

            // If target character does not equal type of attacking character e.g. if player is vampire it doesn't deal damage to vampires
            // If target character is NOT dead
            if (targetCharacter && targetCharacter.CharacterType != c.CharacterType && targetCharacter.Status != CharacterStatus.Dead) {
                inRange = true;
            }
        }

        if (inRange){
            // change cross hair colour to red
            crosshair.color = Color.red;
        }else{
            // change cross hair colour to default white
            crosshair.color = Color.white;
        }


        // Regenerate health if player is still for more than three seconds 

        // If player doesn't move
        if (x == 0 && z == 0){
            healthRegenerateTimer += Time.deltaTime; // health timer increases
        }else{
            healthRegenerateTimer = 0;
        }

        // If player is still and hasn't attacked for more than the timeout 
        if (healthRegenerateTimer >= healthRegenerateTimeout){

            if (PlayerVariables.PlayerDifficulty == Difficulty.Hard){
                c.healHealth(2.0f); // regenerate 3 hp
            }else{
                c.healHealth(1.0f); // regenerate 1 hp in easy mode
            }
            
            healthRegenerateTimer = 0; // reset timer
        }


        // Attack villager or enemy
        if (clickTimeout > 0){
            // Restrict number of times the player can click per second
            clickTimeout -= Time.deltaTime;
            
        }else if (Input.GetMouseButtonDown(0)){
            //Debug.Log("Mouse Clicked, call attack"); 

            healthRegenerateTimer = 0; // reset health timer

            clickTimeout = 1f / maxCPS;
            c.attack(); 
        }

        // If spacebar pressed, activate special ability
        if (Input.GetKeyDown(KeyCode.Space)){

            if (c.CharacterType == CharacterType.Vampire && spaceBarTimeout >= leapCoolDown){
               // Cooldown period 10 seconds for leap
               //Debug.Log("Activate leap!");
               c.leap();

               spaceBarTimeout = 0; // reset

            }else if (c.CharacterType == CharacterType.Zombie && spaceBarTimeout >= stunCoolDown){
                // Cooldown period 30 seconds for stun
                //Debug.Log("Activate stun!");
                c.stun();
                
                spaceBarTimeout = 0; // reset
            }else{
                //Debug.Log("Cannot use ability. Not yet cooled down. Current timeout: " + spaceBarTimeout);
            }   

        }

        spaceBarTimeout += Time.deltaTime;

    }

    // Return cooldown period for special ability
    public float getCooldownPercentage(){
        if (c.CharacterType == CharacterType.Vampire){

            return (this.spaceBarTimeout/this.leapCoolDown);

        }else if (c.CharacterType == CharacterType.Zombie){

            return (this.spaceBarTimeout/this.stunCoolDown);
        }

        return 0f;
    }

    public void setTarget(Character target){}
    
}
