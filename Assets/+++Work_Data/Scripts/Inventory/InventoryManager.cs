using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    private GameState gameState;
    private StateManager stateManager;
    [SerializeField] private InventorySlot[] inventorySlots;

    [Header("Item Description Panel")] 
    [SerializeField] private GameObject itemDescriptionContainer;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemHeaderText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    
    private void Awake()
    {
        gameState = FindObjectOfType<GameState>();
        stateManager = FindObjectOfType<StateManager>();
    }

    public void RefreshInventory()
    {
        List<State> currentStateList = gameState.GetStateList();
        
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (currentStateList.Count == 0)
            {
                inventorySlots[i].TurnOffBorder();
                //#TODO Turn off state description
            }
            
            if (i < currentStateList.Count)
            {
                StateInfo newStateInfo = stateManager.GetStateInfoById(currentStateList[i].id);
                newStateInfo.amount = currentStateList[i].amount;
                inventorySlots[i].SetStateInfo(newStateInfo);
            }
            else
            {
                inventorySlots[i].TurnOnOffVisuals(false);
            }
        }
    }

    public void ShowItemDescrption(StateInfo stateInfo)
    {
        itemImage.sprite = stateInfo.sprite;
        itemHeaderText.SetText(stateInfo.name);
        itemDescriptionText.SetText(stateInfo.description);

    }
}
