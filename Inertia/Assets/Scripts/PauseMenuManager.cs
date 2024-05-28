using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject creditsMenu;

    public static bool isPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }

            else if (isPaused)
            {
                CloseMenus();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;

        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Settings()
    {
        //pauseMenu.SetActive(false);
        //settingsMenu.SetActive(true);
        if(settingsMenu.activeSelf == true)
        {
            settingsMenu.SetActive(false);
        }
        else
        {
            settingsMenu.SetActive(true);
            creditsMenu.SetActive(false);
        }
    }

    public void CloseMenus()
    {
        isPaused = false;

        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void MainMenu()
    {
        isPaused = false;

        Time.timeScale = 1;

        AudioManager.instance.StopAllAudio();

        //Loads the main menu
        SceneManager.LoadScene(0);
    }

    public void Credits()
    {
        if (creditsMenu.activeSelf == true)
        {
            creditsMenu.SetActive(false);
        }
        else
        {
            creditsMenu.SetActive(true);
            settingsMenu.SetActive(false);
        }
    }


}
