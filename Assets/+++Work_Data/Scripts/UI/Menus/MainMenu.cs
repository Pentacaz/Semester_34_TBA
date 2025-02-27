using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private PlayerControllerBakery _controllerBakery;
    private void Awake()
    {
        _controllerBakery = FindObjectOfType<PlayerControllerBakery>();
    }

    private void Start()
    {
        _controllerBakery.OnDisable();
    }

    public void StartButton()
    {
        _controllerBakery.OnEnable();
       SceneManager.LoadScene("Bakery");
       

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
    {   _controllerBakery.OnEnable();
        SceneManager.LoadScene("Bakery");
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");

    }

}
