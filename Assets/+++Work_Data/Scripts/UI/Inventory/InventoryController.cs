using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private InputAction inventoryAction;
    
    private PlayerBaseController _playerBaseController;
   
    private InventoryManager _inventoryManager; //

    #region  EventFunction
    
    public GameObject inventoryObject;
    private void Awake()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();//
        _playerBaseController = FindObjectOfType<PlayerBaseController>();
   
        inputActions = new PlayerInputActions();
        
        //inventoryAction = inputActions.UI.Inventory; //#TODO ADD THIS 
        
   
        inventoryObject.SetActive(false);
    }

    private void OnEnable()
    {
        inventoryAction.performed += OpenInventory;
        EnableInput();
    }

    private void OnDisable()
    {
        inventoryAction.performed -= OpenInventory;
        DisableInput();
    }

    void EnableInput()
    {
        
        inputActions.Enable();
    }

    void DisableInput()
    {
        inputActions.Disable();
        
    }
   #endregion

     #region UiInventory

   private void OpenInventory(InputAction.CallbackContext ctx)
   {
       inventoryObject.SetActive(!inventoryObject.activeSelf);
     
       if (inventoryObject.activeSelf)
       {
          // _inventoryManager.UpdateInventory();//
           _playerBaseController.DisableInput();
       }
       else
       {
           _playerBaseController.EnableInput();
       } 
   }
   #endregion
    
}
