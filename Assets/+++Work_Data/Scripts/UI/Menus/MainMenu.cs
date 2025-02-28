using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject container;
    private PlayerControllerBakery _controllerBakery;
    
    private void Awake()
    {
        container = GameObject.Find("----Essential_Container----");
    }

    private void Start()
    {
       
    }

    public void StartButton()
    {

       SceneManager.LoadScene("Bakery");
       SetActiveContainer();
       

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
        SceneManager.LoadScene("Story");
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");

    }

    public void SetActiveContainer()
    {
        Transform child = container.transform.GetChild(0);
        child.gameObject.SetActive(true);
        
    }
    

}
