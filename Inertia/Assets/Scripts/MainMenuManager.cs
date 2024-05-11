using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void InitialiseGame()
    {
        Invoke("StartGame", 1f);
    }

    public void CloseProgram()
    {
        Application.Quit();
    }

    void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void CloseGame()
    {
        Invoke("CloseProgram", 1f);
    }



}
