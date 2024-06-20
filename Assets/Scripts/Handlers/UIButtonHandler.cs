using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonHandler : MonoBehaviour
{
    public void EnterMainMenu()
    {
        SceneManager.LoadScene("CombatGym");
    }
    public void EnterGame()
    {
        SceneManager.LoadScene("Tutorial");
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
