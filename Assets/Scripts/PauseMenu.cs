using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        paused = false;

        Button resumeButton = transform.GetChild(2).GetComponent<Button>();
        resumeButton.onClick.AddListener(Resume);

        Button quitButton = transform.GetChild(3).GetComponent<Button>();
        quitButton.onClick.AddListener(Quit);

        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused == false) Pause();
            else Resume();
        }
    }

    private void Pause()
    {
        paused = true;
        Time.timeScale = 0;
        for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(true);
    }

    private void Resume()
    {
        paused = false;
        Time.timeScale = 1;
        for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(false);
    }

    private void Quit()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
