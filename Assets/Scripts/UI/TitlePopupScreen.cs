using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitlePopupScreen : MonoBehaviour
{
    private GameObject currentScreen;
    private GameObject titleScreen;
    private GameObject helpScreen;
    private GameObject manualHelpScreen;
    private GameObject towerHelpScreen;
    private GameObject enemyHelpScreen;

    // Start is called before the first frame update
    void Start()
    {

        titleScreen = transform.GetChild(0).gameObject;
        titleScreen.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(GameStart);
        titleScreen.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Help);

        helpScreen = transform.GetChild(1).gameObject;
        helpScreen.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(ManualHelp);
        helpScreen.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(TowerHelp);
        helpScreen.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(EnemyHelp);
        helpScreen.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(Title);

        manualHelpScreen = transform.GetChild(2).gameObject;
        manualHelpScreen.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Help);

        towerHelpScreen = transform.GetChild(3).gameObject;
        towerHelpScreen.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Help);

        enemyHelpScreen = transform.GetChild(4).gameObject;
        enemyHelpScreen.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Help);

        SetCurrentScreen(titleScreen);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentScreen == helpScreen)
            {
                Title();
            }
            else if (currentScreen == manualHelpScreen || currentScreen == towerHelpScreen || currentScreen == enemyHelpScreen)
            {
                Help();
            }
        }
    }

    void Title()
    {
        SetCurrentScreen(titleScreen);
    }

    void GameStart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void Help()
    {
        SetCurrentScreen(helpScreen);
    }

    private void ManualHelp()
    {
        SetCurrentScreen(manualHelpScreen);
    }

    private void TowerHelp()
    {
        SetCurrentScreen(towerHelpScreen);
    }

    private void EnemyHelp()
    {
        SetCurrentScreen(enemyHelpScreen);
    }

    private void SetCurrentScreen(GameObject screen)
    {
        currentScreen = screen;
        foreach (Transform child in transform) child.gameObject.SetActive(false);
        if (currentScreen != null)
        {
            currentScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
