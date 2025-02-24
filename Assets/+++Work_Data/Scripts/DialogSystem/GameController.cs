using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //private PlayerController player;
    private PlayerBaseController playerBaseController;
    private DialogueController dialogueController;

    public enum GameMode
    { PreMenu, MainMenu, NewGame, LoadGame,GameMode,DebugMode }

    public GameMode gameMode;
    
    
    public Button lastSelectable;
    #region Unity Event Functions

    private void Awake()
    {
        //player = FindObjectOfType<PlayerController>();
        playerBaseController = FindObjectOfType<PlayerBaseController>();
        dialogueController = FindObjectOfType<DialogueController>();
    }

    private void OnEnable()
    {
        DialogueController.DialogueClosed += EndDialogue;
    }

    private void Start()
    {
        //if(player)
           // EnterPlayMode();

           if (playerBaseController)
           {
               EnterPlayMode();
           }
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
        //player.OnEnable();
        playerBaseController.OnEnable();
    }

   public void EnterDialogueMode()
    {
        // 1 = spiel läuft weiter, 0 = game freeze
        Time.timeScale = 1;
        //Cursor.lockState = CursorLockMode.Locked;
        //player.OnDisable(); 
        playerBaseController.OnDisable();
    }

    public void EnterStatePopUpMode()
    {
        
        Time.timeScale = 1;
        //player.OnDisable(); 
        //playerBaseController.OnDisable();
    }

    // inventory
    public void EnterInventoryMode()
    {
        Time.timeScale = 0;
        //player.OnDisable(); 
        playerBaseController.OnDisable();
    }
   
    #endregion
// Metoden aufruf
    public void StartDialogue(string dialoguePath)
    {// with find typ of object
        EnterDialogueMode();
        dialogueController.StartDialogue(dialoguePath);
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
        StartCoroutine(DelayNewSelectable(newSelectable));
    }

    public void ExitMenu()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    IEnumerator DelayNewSelectable(Selectable newSelectable)
    {
        yield return null;
        newSelectable.Select();
    }
}

