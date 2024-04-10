using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonHandler : MonoBehaviour
{
    public void EnterMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void EnterGame()
    {
        SceneManager.LoadScene("Debug");
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
