using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Play button defined in inspector

    public void QuitGame () {

        Debug.Log("Game was quit");
        Application.Quit(); // This is not visible in Unity editor

    }
}
