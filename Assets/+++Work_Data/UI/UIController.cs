using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private InputAction openInventoryAction;
    private InputAction openMenuAction;
    private PlayerController playerController;
    private InventoryManager inventoryManager;
    private bool inventoryCanOpen;
    private bool menuCanOpen;


    public GameObject inventoryUI;
    public GameObject menuUI;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
       // openInventoryAction = inputActions.UI.Inventory;
       // openMenuAction = inputActions.UI.Menu;
        playerController = Component.FindObjectOfType<PlayerController>();
        inventoryManager = Component.FindObjectOfType<InventoryManager>();
    }


    private void OnEnable()
    {
        EnableUiInput();
        openInventoryAction.performed += OpenInventory;
        openInventoryAction.canceled += OpenInventory;
        openMenuAction.performed += OpenMenu;
        openMenuAction.canceled += OpenMenu;
    }

    private void OnDisable()
    {
        DisableUiInput();
        openInventoryAction.performed -= OpenInventory;
        openInventoryAction.canceled -= OpenInventory;
        
        openMenuAction.performed -= OpenMenu;
        openMenuAction.canceled -= OpenMenu;
    }
    
    public void EnableUiInput()
    {
        inputActions.Enable();
    }

    public void DisableUiInput()
    {
        inputActions.Disable();
    }

    void OpenInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inventoryCanOpen = !inventoryCanOpen;
            if (inventoryCanOpen)

            {
                playerController.OnEnable();
                inventoryUI.SetActive(false);

            }
            else
            {
                playerController.OnDisable();
                inventoryUI.SetActive(true);
                inventoryManager.RefreshInventory();


            }
        }
    }

    void OpenMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            menuCanOpen = !menuCanOpen;
            if (menuCanOpen)

            {
                playerController.OnEnable();
                menuUI.SetActive(false);

            }
            else
            {
                playerController.OnDisable();
                menuUI.SetActive(true);

            }
        }
    }
}