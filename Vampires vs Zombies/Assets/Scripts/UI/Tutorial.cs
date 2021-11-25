using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject welcomePage;
    public GameObject bgdOnePage;
    public GameObject bgdTwoPage;
    public GameObject premisePage;
    public GameObject controlsPage;
    public GameObject specialPage;
    public GameObject uiExplainPage;
    public GameObject enjoyGamePage;
    public GameObject crosshair;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.parent.gameObject.GetComponent<MainSceneUI>().isTutorial = true; 
        crosshair.SetActive(false);
        LoadWelcome();
        
    }

    public void LoadWelcome() {
        // enable correct page
        welcomePage.SetActive(true);

        bgdOnePage.SetActive(false);
        bgdTwoPage.SetActive(false);
        premisePage.SetActive(false);
        controlsPage.SetActive(false);
        specialPage.SetActive(false);
        uiExplainPage.SetActive(false);
        enjoyGamePage.SetActive(false);

    }

    public void LoadBgdOne() {
        // enable correct page
        welcomePage.SetActive(false);

        bgdOnePage.SetActive(true);

        bgdTwoPage.SetActive(false);
        premisePage.SetActive(false);
        controlsPage.SetActive(false);
        specialPage.SetActive(false);
        uiExplainPage.SetActive(false);
        enjoyGamePage.SetActive(false);
    }
    
    public void LoadBgdTwo() {
        // enable correct page
        welcomePage.SetActive(false);
        bgdOnePage.SetActive(false);

        bgdTwoPage.SetActive(true);

        premisePage.SetActive(false);
        controlsPage.SetActive(false);
        specialPage.SetActive(false);
        uiExplainPage.SetActive(false);
        enjoyGamePage.SetActive(false);
    }

    public void LoadPremise() {
        // enable correct page
        welcomePage.SetActive(false);
        bgdOnePage.SetActive(false);
        bgdTwoPage.SetActive(false);
        
        premisePage.SetActive(true);

        controlsPage.SetActive(false);
        specialPage.SetActive(false);
        uiExplainPage.SetActive(false);
        enjoyGamePage.SetActive(false);
    }

    public void LoadControls() {
        // enable correct page
        welcomePage.SetActive(false);
        bgdOnePage.SetActive(false);
        bgdTwoPage.SetActive(false);
        premisePage.SetActive(false);

        controlsPage.SetActive(true);
        
        specialPage.SetActive(false);
        uiExplainPage.SetActive(false);
        enjoyGamePage.SetActive(false);
    }

    public void LoadSpecialControls() {
        // enable correct page
        welcomePage.SetActive(false);
        bgdOnePage.SetActive(false);
        bgdTwoPage.SetActive(false);
        premisePage.SetActive(false);
        controlsPage.SetActive(false);

        specialPage.SetActive(true);
        
        uiExplainPage.SetActive(false);
        enjoyGamePage.SetActive(false);
    }

    public void LoadUIExplain() {
        // enable correct page
        welcomePage.SetActive(false);
        bgdOnePage.SetActive(false);
        bgdTwoPage.SetActive(false);
        premisePage.SetActive(false);
        controlsPage.SetActive(false);
        specialPage.SetActive(false);

        uiExplainPage.SetActive(true);

        enjoyGamePage.SetActive(false);
    }

    public void LoadEnjoyPage() {
        // enable correct page
        welcomePage.SetActive(false);
        bgdOnePage.SetActive(false);
        bgdTwoPage.SetActive(false);
        premisePage.SetActive(false);
        controlsPage.SetActive(false);
        specialPage.SetActive(false);
        uiExplainPage.SetActive(false);

        enjoyGamePage.SetActive(true);
    }

    //void StartGame() {}

    public void StartGame() { //also used for Skip button
        // disable all tutorial pages
        welcomePage.SetActive(false);
        bgdOnePage.SetActive(false);
        bgdTwoPage.SetActive(false);
        premisePage.SetActive(false);
        controlsPage.SetActive(false);
        specialPage.SetActive(false);
        uiExplainPage.SetActive(false);
        enjoyGamePage.SetActive(false);

        crosshair.SetActive(true);

        //unfreeze time 
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        transform.parent.gameObject.GetComponent<MainSceneUI>().isTutorial = false;
        // is that all that's needed?
    }
}
