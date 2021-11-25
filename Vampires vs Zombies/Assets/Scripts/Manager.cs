using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    // INITIALISING VALUES

    public Terrain terrain;
    public GameObject characterTemplate;
    public List<Character> characters = new List<Character>();

    public int numZombies = 0;
    public int numVampires = 0;

    // Special ability cool down UI reference
    public Image specialCooldown;
    private Color filledColour;

    // Variables for managing player's health bar
    private Character playerCharacter;
    private HealthBarController playerHealthBarControl;

    // Variables for managing progress bars
    private ProgressController vampireProgress;
    private ProgressController zombieProgress;

    // Camera control
    public MouseVisionController mouseVisionController;

    // Models to be spawned
    private string[] modelNames = {"BoyModel", "ManModel", "LadySkirtModel", "LadyPantsModel"};
    private GameObject[] modelPrefabs = new GameObject[4];

    // Reference to the music player
    private MusicPlayer backgroundMusic;


    private static Manager _instance;
    public static Manager Instance {
        get  {
            if (!_instance) {
                GameObject gameManager = new GameObject();
                _instance = gameManager.AddComponent<Manager>();
            }
            return _instance;
        }
    }


    // WHEN OBJECT IS CREATED
    void Awake() {
        if (!_instance) {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this);
        }
    }


    private Vector3 spawnPosition() {
        // random world space location
        float x = Random.Range(370, 430);
        float z = Random.Range(270, 330);
        float y = terrain.SampleHeight(new Vector3(x, 0, z));
        return new Vector3(x, y, z) + Vector3.up * 2; // add body height
    }

    // WHEN SCENE IS OPENED/STARTED
    void Start()
    {   
        // Pause time at start of game
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None; // can use mouse as normal (not in game)

        // Set active terrain
        terrain = Terrain.activeTerrain;

        // Set playerUi & tutorial to active? 

        // Load Model Prefabs
        for (int i=0; i<modelNames.Length; i++) {
            modelPrefabs[i] = (GameObject)Resources.Load("Character Prefabs/Models/" + modelNames[i], typeof(GameObject));
        }

        // Load Camera
        GameObject camera = GameObject.Find("Main Camera");
        GameObject player;

        // Initialise Player's Type (Zombie or Vampire)
        if (PlayerVariables.isZombie){
            player = spawnCharacters(CharacterType.Zombie, 1);

        }else{
            player = spawnCharacters(CharacterType.Vampire, 1);
        }

        // Add player sounds
        player.AddComponent<AudioSource>();
        player.GetComponent<AudioSource>().spatialBlend = 1.0f; // 3D sound

        //Initialise player object
        playerCharacter = player.GetComponent<Character>();
        playerCharacter.controlBehaviour = new PlayerController(playerCharacter);
        playerCharacter.damageBehaviour = new DefaultDamage(playerCharacter, 20f); // Starting Health


        // Attach camera to player
        camera.transform.parent = player.transform;
        camera.transform.localPosition = new Vector3(0,0.5f,0.2f); // attach camera slightly forwards
        mouseVisionController = camera.AddComponent<MouseVisionController>();
        mouseVisionController.player = player.transform;

        // Set Player Health Bar
        GameObject playerHealthBar = GameObject.Find("PlayerHealthBar");
        playerHealthBarControl = playerHealthBar.GetComponent<HealthBarController>();

        // Disable hovering health bar for player 
        playerCharacter.transform.Find("HealthBarCanvas").gameObject.SetActive(false);
      
        // if player is a zombie
        if (PlayerVariables.isZombie){
            // Health bar colour
            playerHealthBarControl.SetColour(Color.green);

            // Cool down UI
            specialCooldown = GameObject.Find("Stun").GetComponent<Image>();
            filledColour = new Color(0.51f,0.63f,0.53f);

            // Higher damage but move slower
            playerCharacter.attackBehaviour = new PlayerAttack(playerCharacter, camera, 1.5f);
            playerCharacter.moveBehaviour = new DefaultMove(playerCharacter, 5.5f);
            

            if (PlayerVariables.PlayerDifficulty == Difficulty.Hard){
                // Hard difficulty  
                spawnCharacters(CharacterType.Vampire, 3); // spawn three enemies

            }else if (PlayerVariables.PlayerDifficulty == Difficulty.Easy){
                // Default easy difficulty
                spawnCharacters(CharacterType.Vampire, 2); // spawn two enemy
            }
            
        // if player is a vampire
        } else {

            playerHealthBarControl.SetColour(Color.cyan);

            // Cool down UI
            specialCooldown = GameObject.Find("Leap").GetComponent<Image>();
            filledColour = new Color(0.4f,0.75f,0.91f);

            // Lower damage but move faster
            playerCharacter.attackBehaviour = new PlayerAttack(playerCharacter, camera, 1.0f);
            playerCharacter.moveBehaviour = new DefaultMove(playerCharacter, 7.0f);

            if (PlayerVariables.PlayerDifficulty == Difficulty.Hard){
                // Hard difficulty  
                spawnCharacters(CharacterType.Zombie, 3); // spawn three enemies

            }else if (PlayerVariables.PlayerDifficulty == Difficulty.Easy){
                // Default easy difficulty
                spawnCharacters(CharacterType.Zombie, 2); // spawn two enemy
            }

        }
        

        // Load progress bar
        zombieProgress = GameObject.Find("ZombieProgress").GetComponent<ProgressController>();
        vampireProgress = GameObject.Find("VampireProgress").GetComponent<ProgressController>();

        // Spawn villagers

        // Spawn 20 villagers in groups of 5 
        for (int i=0; i< 5; i++) {
            Vector3 position = spawnPosition();
            spawnCharacters(CharacterType.Villager, 5, position); // spawn villagers at the moment 
        }
        

        // Start background music
        backgroundMusic = GameObject.Find("Music").GetComponent<MusicPlayer>();
        if (backgroundMusic != null) {
            backgroundMusic.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if (backgroundMusic != null) {
            backgroundMusic.source.volume = PlayerVariables.gameVolume;
        }
        
        if (Time.timeScale == 0f) {
            return;
        }

        // Remove dead characters
        characters.RemoveAll((Character c) => c.Status == CharacterStatus.Dead);

        // Make them act
        foreach(Character c in characters) {
            if (c.Status != CharacterStatus.Dead){
                c.act();
            }
        }

        numVampires = characters.Count(x => x.CharacterType == CharacterType.Vampire);
        numZombies = characters.Count(x => x.CharacterType == CharacterType.Zombie);

        // Check if game has finished
        if (((numZombies+numVampires) == characters.Count)) {
            //Delay before game is finished
            ExecuteAfter((() => {
                PlayerVariables.isGameFinished = true;

                // Update whether they win or not
                if (playerCharacter.CharacterType == CharacterType.Vampire) { // if vampire
                    if (numVampires > numZombies) {
                        PlayerVariables.wonGame = true;
                    } else {
                        PlayerVariables.wonGame = false;
                    }
                
                } else {
                    if (numZombies > numVampires) { // if zombie
                        PlayerVariables.wonGame = true;
                    } else {
                        PlayerVariables.wonGame = false;
                    }

                }

            }), 3);
            
        } 

        // check if player killed enemy - game over
        if (PlayerVariables.isZombie) {
            if (numVampires == 0) {
                // game won against vampires
                ExecuteAfter((() => {
                    PlayerVariables.isGameFinished = true;
                    PlayerVariables.wonGame = true;
                }), 3);
            }
        } else {
            if (numZombies == 0) {
                // game won against zombies
                ExecuteAfter((() => {
                    PlayerVariables.isGameFinished = true;
                    PlayerVariables.wonGame = true;
                }), 3);
            }
        }

        // check if player died 
        if (playerCharacter.GetHealthPercentage() < 0.1f) {
            // Game is finished and they have NOT won the game - dead
            ExecuteAfter((() => {
                PlayerVariables.isGameFinished = true;
                PlayerVariables.wonGame = false;
                PlayerVariables.isDead = true;
            }), 3);
        }

        if (PlayerVariables.isGameFinished)
        {
            // stop the music
            backgroundMusic.Stop();
        }

        // Check and set player's health for each update
        if (playerCharacter){
            playerHealthBarControl.SetHealth(playerCharacter.GetHealthPercentage());

            // Set cooldown timer 
            float coolDown = ((PlayerController)playerCharacter.controlBehaviour).getCooldownPercentage();

            if (coolDown >= 1){
                specialCooldown.fillAmount = 1.0f;
                specialCooldown.color = filledColour; // cooldown changes colour
            }else{
                specialCooldown.fillAmount = coolDown;
                specialCooldown.color = Color.white;
            }
            
            
        }

        // Check and set progress bars each update
        vampireProgress.SetProgress((float)numVampires/(float)characters.Count);
        zombieProgress.SetProgress((float)numZombies/(float)characters.Count);
    
        while (toExecute.Count > 0 && Time.fixedTime > toExecute.Keys[0].Item1) {
            toExecute.Values[0]();
            toExecute.RemoveAt(0);

        }
        
    }

    // Method for spawning characters
    public GameObject spawnCharacters(CharacterType type, int n, Vector3? position = null) {
        GameObject spawn = null;
        for (int i=0; i<n; i++) {
            // Instantiate villager
            spawn = GameObject.Instantiate<GameObject>(characterTemplate);
            spawn.transform.parent = this.transform;

            //spawn.transform.position = position ?? spawnPosition() + new Vector3(Random.value * 2 - 1, 0, Random.value * 2 - 1);
            spawn.transform.position = position ?? spawnPosition(); // spawn at body height

            // Add the model
            int useModel = (int)System.Math.Floor(Random.value * 4.0f);
            GameObject model = Instantiate(modelPrefabs[useModel], new Vector3(0,0,0), spawn.transform.rotation);
            model.transform.parent = spawn.transform;
            model.transform.localPosition = new Vector3(0,-1.1f,0);

            // Setup the attached character script
            Character spawnCharacter = spawn.GetComponent<Character>();
            spawnCharacter.CharacterType = type; 
            characters.Add(spawnCharacter);

            // Add mutator script to the model
            Mutate mutator = model.AddComponent<Mutate>();

            // Change material of starting characters
            // Get mesh renderers
            SkinnedMeshRenderer[] meshes = spawn.GetComponentsInChildren<SkinnedMeshRenderer>();

            mutator.MutateInto(type, 0);

            // Add sounds
            spawn.AddComponent<AudioSource>();
            spawn.GetComponent<AudioSource>().spatialBlend = 1.0f; // 3D sound

        }
        return spawn;

    }


    private SortedList<(float, int), System.Action> toExecute = new SortedList<(float, int), System.Action>();
    private int keyComposite = 0;

    // Execute action after time seconds
    public void ExecuteAfter(System.Action func, float time) {
        toExecute.Add((Time.fixedTime + time, keyComposite), func);
        keyComposite++;
        
    }
}
