using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventorySlot[] inventorySlots;
    private GameState gameState;
    private StateManager stateManager;


   
   
   
   
    private void Awake()
    {
        gameState = FindObjectOfType<GameState>();
        stateManager = FindObjectOfType<StateManager>();
        
        RefreshInventory();
    }
   
    public void RefreshInventory()
    {
        List<State> currentStateList = gameState.GetStateList();
      
     
        for (int i = 0; i < inventorySlots.Length; i++)
        {

            if (currentStateList.Count == 0)
            {
               // inventorySlots[i].TurnOffBorder();
            }
         
         
            if (inventorySlots != null && i < currentStateList.Count)
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



}