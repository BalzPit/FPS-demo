using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //keep track of whether or not the gae is paused
    public static bool gameIsPaused = false;

    int menuSceneIndex = 0;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                //game is already paused, resume
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked; //make cursor disappear

        pauseMenuUI.SetActive(false);

        //set time ack to normal
        Time.timeScale = 1f;

        gameIsPaused = false;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None; //makes menu interactable

        pauseMenuUI.SetActive(true);

        //freeze time
        Time.timeScale = 0f;

        gameIsPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneIndex);
    }
}
