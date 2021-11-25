using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Reference: PAUSE MENU in Unity by Brackeys https://www.youtube.com/watch?v=JivuXdrIHK0&list=PLZb-pbd1-DXm7xq5cM9wysZEsf8VNzOTO&index=6&ab_channel=Brackeys 

public class MainSceneUI : MonoBehaviour
{
    public static bool isPaused = false;

    public bool isTutorial = true;
    public GameObject pauseMenuUI;
    public GameObject playerUI;
    public GameObject gameWonMenu;
    public GameObject gameOverMenu;
    public GameObject zombieText;
    public GameObject vampireText;
    public GameObject deadText;
    public GameObject youLostext;

    // Update is called once per frame
    void Update()
    {
        // check if pause menu is triggered
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused && !isTutorial) { // disable changing to pause menu when tutorial is active
                Resume();
            } else if (!isTutorial) {
                Pause();
            }
        }

        // check if game is finished
        if (PlayerVariables.isGameFinished) {
            if (PlayerVariables.wonGame) {
                GameWon();
            } else {
                GameOver();
            }

        }
    }

    public void GameOver() {

        // Pause time
        Time.timeScale = 0f;
        isPaused = true; // dont think this does anything
        Cursor.lockState = CursorLockMode.None;

        // disable player UI and pause menu to be safe
        playerUI.SetActive(false);
        pauseMenuUI.SetActive(false);

        //enable game over menu
        gameOverMenu.SetActive(true);

        // If you died
        if (PlayerVariables.isDead) {

            // Display "you died" text
            deadText.SetActive(true);

            // make sure others are off to be safe
            youLostext.SetActive(false);
            vampireText.SetActive(false);
            zombieText.SetActive(false);

        // If you lost and you're a zombie
        } else if (PlayerVariables.isZombie) {

            // Display "you lost to vampires"
            youLostext.SetActive(true);
            vampireText.SetActive(true);

            // make sure others are off to be safe
            deadText.SetActive(false);
            zombieText.SetActive(false);

        // If you lost and you're a vampire
        } else {

            //Display "you lost to zombies"
            youLostext.SetActive(true);
            zombieText.SetActive(true);

            // make sure others are off to be safe
            vampireText.SetActive(false);
            deadText.SetActive(false);
        }
    }
    public void GameWon () {

        // Pause time
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;

        // disable player UI and pause menu to be safe
        playerUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        gameOverMenu.SetActive(false);
        
        //enable game won menu
        gameWonMenu.SetActive(true);
    }

    public void Resume() {
        // Disable pause menu
        pauseMenuUI.SetActive(false);
        // Activate player UI
        playerUI.SetActive(true);

        // Start time again
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
    void Pause() {
        // Activate pause Menu
        pauseMenuUI.SetActive(true);
        // Disable player UI
        playerUI.SetActive(false);
        
        // Pause time
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadMainMenu() {
        // Time starts again
        Time.timeScale = 1f;
        isPaused = false;
        // However don't lock cursor
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);

    }

    public void QuitGame() {
        Debug.Log("Quit Game");
        Application.Quit(); // This is not visible in Unity editor

    }
}
