using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject container;
    public GameObject options;

    
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
        
    }

    public void OptionsButton()
    {
     
        options.SetActive(true);
      
    }

    public void NewGameButton()
    {   
        SceneManager.LoadScene("Story");
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
        options.SetActive(false);
   
    }

    public void SetActiveContainer()
    {
        Transform child = container.transform.GetChild(0);
        child.gameObject.SetActive(true);
        
    }
    

}
