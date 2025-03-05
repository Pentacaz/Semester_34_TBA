using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public GameObject optionsMenuContainer;
    public PlayerInputActions inputActions;
    private InputAction pauseAction;

    

    private void Awake()
    {

        inputActions = new PlayerInputActions();
        pauseAction = inputActions.Player.Menu;
       
    }


    private void OnEnable()
    {
        EnableInput();
        inputActions.Enable();

        pauseAction.performed += OpenOptions;

        
    }

    private void OnDisable()
    {
        DisableInput();
        inputActions.Disable();

        pauseAction.performed -= OpenOptions;

        
    }


    public void DisableInput()
    {
        inputActions.Disable();
        
    }

    public void EnableInput()
    {
        inputActions.Enable();
    }


    public void OpenOptions(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            optionsMenuContainer.SetActive(!optionsMenuContainer.activeInHierarchy);
            Debug.Log("Open");
            
            if (optionsMenuContainer.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
            
        }
        
        

    }
    
    public void RestartButton()
    {
        SceneManager.LoadScene("Test_Scene");

    }
    
    public void OptionsButton()
    {
        SceneManager.LoadScene("Options");

    }
    
    public void QuitButton()
    {
        SceneManager.LoadScene("MainMenu");

    }
    
    public void ExitButton()
    {
        Application.Quit();
    }
}
