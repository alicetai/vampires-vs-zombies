using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    
    public Button zombButton;
    public Button vampButton;
    public Button easyButton;
    public Button hardButton;

    public Button playButton;
    public Button backButton;

    private bool isZombie = false; // defaults to Vampire
    private bool isEasy = true; // defaults to Easy

   // Reference: Select multiple buttons - Unity Forum https://forum.unity.com/threads/select-multiple-buttons.1090780/ 
   // Disable buttons instead of using select to "select" multiple buttons - just have the same animation visually

    public void buttonClicked(Button button) 
    {
        if (button == zombButton) {
            //Debug.Log("zombie button");
            isZombie = true;
            zombButton.interactable = false;
            vampButton.interactable = true;

        } else if (button == vampButton) {
            isZombie = false;
            zombButton.interactable = true;
            vampButton.interactable = false;

        } else if (button == easyButton) {
            isEasy = true;
            easyButton.interactable = false;
            hardButton.interactable = true;

        } else if (button == hardButton) {
            isEasy = false;
            easyButton.interactable = true;
            hardButton.interactable = false;
        }

    }

    // No button effect - just load new scene
    public void PlayGame () {
        if (isZombie) {
            
            // Save to static class
            PlayerVariables.PlayerType = CharacterType.Zombie;
            PlayerVariables.isZombie = true;

        } else {

            // Save to static class
            PlayerVariables.PlayerType = CharacterType.Vampire;
            PlayerVariables.isZombie = false;
        }

        if (isEasy) {

            // Save to static class
            PlayerVariables.PlayerDifficulty = Difficulty.Easy;
        } else {
            PlayerVariables.PlayerDifficulty = Difficulty.Hard;
        }

        // Loading next scene in queue
        // Queue is found inside Build Settings (File -> Build Settings);
        SceneManager.LoadScene(1);
    }

    public void goBack () {
        zombButton.interactable = true;
        vampButton.interactable = true;
        easyButton.interactable = true;
        hardButton.interactable = true;
    }
}
