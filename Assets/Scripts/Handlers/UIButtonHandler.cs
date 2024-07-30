using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonHandler : MonoBehaviour
{
    public void EnterTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void EnterTheGrid()
    {
        SceneManager.LoadScene("TheGrid");
    }

    public void EnterTheGarden()
    {
        SceneManager.LoadScene("TheGarden");
    }
    public void EnterSettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
