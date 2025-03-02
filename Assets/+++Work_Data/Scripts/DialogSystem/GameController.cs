using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PlayerControllerBakery[] player;
    public PlayerBaseController[] pcontroller;
    private DialogueController dialogueController;

    public enum GameMode
    { PreMenu, MainMenu, NewGame, LoadGame,GameMode,DebugMode }

    public GameMode gameMode;

    private GameObject _bakeryIndicator;
    private GameObject _dungeonIndicator;
    
    public Button lastSelectable;
    #region Unity Event Functions

    private void Awake()
    {
        //player = Resources.FindObjectsOfTypeAll<PlayerControllerBakery>();
        pcontroller = Resources.FindObjectsOfTypeAll<PlayerBaseController>();
        dialogueController = FindObjectOfType<DialogueController>();
        
        
    }

    

    private void OnEnable()
    {
        DialogueController.DialogueClosed += EndDialogue;
    }

    private void Start()
    {
        _bakeryIndicator = GameObject.Find("BakeryIndicator");
        _dungeonIndicator = GameObject.Find("DungeonIndicator");
        
        if (_bakeryIndicator != null)
        {
            if (player != null)
            {
                EnterPlayMode();
            }
        }else if (_dungeonIndicator != null)
        {
            if (pcontroller != null)
            {
                EnterPlayMode();
            }
        }
        
    }

    private void Update()
    {
        _bakeryIndicator = GameObject.Find("BakeryIndicator");
        _dungeonIndicator = GameObject.Find("DungeonIndicator");
    }

    private void OnDisable()
    {
        DialogueController.DialogueClosed -= EndDialogue;
    }

    #endregion

    #region Modes

    public void EnterPlayMode()
    {
        Time.timeScale = 1;
        // In the editor: Unlock with ESC.
        //Cursor.lockState = CursorLockMode.Locked;
        // foreach (var cntrol in player)
        // {
        // cntrol.OnEnable();
        // }
        
        if (_bakeryIndicator != null)
        {
            foreach (var cntrol in player)
            {
                cntrol.OnEnable();
            }
        }
        else if (_dungeonIndicator != null)
        {
            foreach (var cntrol in pcontroller)
            {
                cntrol.OnEnable();
            }
        }


    }

    public void EnterDialogueMode()
    {
        // 1 = spiel läuft weiter, 0 = game freeze
        Time.timeScale = 1;
        //Cursor.lockState = CursorLockMode.Locked;
        
      if (_bakeryIndicator != null)
      {
          foreach (var cntrol in player)
          {
              cntrol.OnDisable();
          }
      }
      else if (_dungeonIndicator != null)
      {
          foreach (var cntrol in pcontroller)
          {
              cntrol.OnDisable();
          }
      }
    }

    public void EnterStatePopUpMode()
    {

        Time.timeScale = 1;
       // foreach (var cntrol in player)
        //{
           // cntrol.OnDisable();
       // }
        
       if (_bakeryIndicator != null)
       {
           foreach (var cntrol in player)
           {
               cntrol.OnDisable();
           }
       }
       else if (_dungeonIndicator != null)
       {
           foreach (var cntrol in pcontroller)
           {
               cntrol.OnDisable();
           }
       }
    }

    // inventory
    public void EnterInventoryMode()
    {
        Time.timeScale = 0;
        //player.OnDisable();
        
       if (_bakeryIndicator != null)
       {
           foreach (var cntrol in player)
           {
               cntrol.OnDisable();
           }
       }
       else if (_dungeonIndicator != null)
       {
           foreach (var cntrol in pcontroller)
           {
               cntrol.OnDisable();
           }
       }
    }

    #endregion
// Methoden aufruf
    public void StartDialogue(string dialoguePath)
    {// with find typ of object
        // EnterDialogueMode();
        dialogueController.StartDialogue(dialoguePath);
      
       if (_bakeryIndicator != null)
       {
           foreach (var cntrol in player)
           {
               cntrol.OnDisable();
           }
       }
       else if (_dungeonIndicator != null)
       {
           foreach (var cntrol in pcontroller)
           {
               cntrol.OnDisable();
           }
       }
    }

    public void StartStatePopUp()
    {
        // ruft mur methoden auf 
        // dirigiert nur 
        EnterStatePopUpMode();
        
        
    }
// iventory
    public void StartOpenInventory()
    {
        EnterInventoryMode();

    }

    public void EndCloseInventory()
    {
        
        EnterPlayMode();
    }
    
    public void CloseStatePopUp()
    {
        EnterPlayMode();
    }


    private void EndDialogue()
    {// aktion event
        
        EnterPlayMode();
        if (_bakeryIndicator != null)
        {
            foreach (var cntrol in player)
            {
                cntrol.OnEnable();
            }
        }
        else if (_dungeonIndicator != null)
        {
            foreach (var cntrol in pcontroller)
            {
                cntrol.OnEnable();
            }
        }
     
    }

    public void SetLastSelectable()
    {
        SetSelectable(lastSelectable);
    }
    
    public void SetSelectable(Button newSelactable)
    {
        Selectable newSelectable;
        lastSelectable = newSelactable;
        newSelectable = newSelactable;

        //newSelactable.Select();
        StartCoroutine(DelaySetSelectable(newSelectable));
    }

    public void ExitMenu()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

   
  /*  
    public void SetSelectable(Button button)
    {
        Selectable selectable = button;

        StartCoroutine(DelaySetSelectable(selectable));

    }
    */

    IEnumerator DelaySetSelectable(Selectable selectable)
    {
        yield return null;
        selectable.Select();
    }
}

