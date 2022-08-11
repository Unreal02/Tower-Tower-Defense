using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopupScreen : MonoBehaviour
{
    private GameObject currentScreen; // null은 팝업 스크린이 없음을 의미
    private GameObject pauseScreen;
    private GameObject gameWinScreen;
    private GameObject gameLoseScreen;
    private GameObject helpScreen;

    // Start is called before the first frame update
    void Start()
    {
        currentScreen = null;
        Time.timeScale = 1;

        // Pause Button
        Button pauseButton = GameObject.Find("Pause Button").GetComponent<Button>();
        pauseButton.onClick.AddListener(Pause);

        pauseScreen = transform.GetChild(0).gameObject;
        pauseScreen.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Resume);
        pauseScreen.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Restart);
        pauseScreen.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(Help);
        pauseScreen.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(Quit);

        gameWinScreen = transform.GetChild(1).gameObject;
        gameWinScreen.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Restart);
        gameWinScreen.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Quit);

        gameLoseScreen = transform.GetChild(2).gameObject;
        gameLoseScreen.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Restart);
        gameLoseScreen.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Quit);

        helpScreen = transform.GetChild(3).gameObject;
        helpScreen.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Pause);

        PlayerInfo playerInfo = FindObjectOfType<PlayerInfo>();
        playerInfo.onGameLose.AddListener(GameLose);

        RoundManager roundManager = FindObjectOfType<RoundManager>();
        roundManager.onGameWin.AddListener(GameWin);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentScreen == null)
            {
                Pause();
            }
            else if (currentScreen == pauseScreen)
            {
                Resume();
            }
            else if (currentScreen == helpScreen)
            {
                Pause();
            }
        }
    }

    private void GameWin()
    {
        Time.timeScale = 0;
        SetCurrentScreen(gameWinScreen);
    }

    private void GameLose()
    {
        Time.timeScale = 0;
        SetCurrentScreen(gameLoseScreen);
    }

    private void Pause()
    {
        Time.timeScale = 0;
        SetCurrentScreen(pauseScreen);
    }

    private void Resume()
    {
        Time.timeScale = 1;
        SetCurrentScreen(null);
    }

    private void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void Help()
    {
        Time.timeScale = 0;
        SetCurrentScreen(helpScreen);
    }

    private void Quit()
    {
        SceneManager.LoadScene("TitleScene");
    }

    private void SetCurrentScreen(GameObject screen)
    {
        currentScreen = screen;
        foreach (Transform child in transform) child.gameObject.SetActive(false);
        if (currentScreen != null)
        {
            currentScreen.SetActive(true);
        }
    }
}
