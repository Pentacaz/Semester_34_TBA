using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartButton()
    {
       SceneManager.LoadScene("Test_scene");

    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OptionsButton()
    {
        SceneManager.LoadScene("Options");

    }

    public void NewGameButton()
    {
        SceneManager.LoadScene("Test_scene");
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");

    }

}
