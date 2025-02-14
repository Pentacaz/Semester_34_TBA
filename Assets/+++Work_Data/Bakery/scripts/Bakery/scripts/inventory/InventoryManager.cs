using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManagerBakes : MonoBehaviour
{
   [SerializeField] private InventorySlot[] inventorySlots;
   [SerializeField] private GameObject inventoryContainer;
   private GameState gameState;
   private StateManager stateManager;

   [Header("Item Description Panel")]
   [SerializeField] private Image itemImage;
   [SerializeField] private GameObject itemDescriptionContainer;
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
            itemDescriptionContainer.SetActive(false);
            inventorySlots[i].TurnOffBorder();
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


   public void ShowItemDescription(StateInfo stateInfo)
   {
      itemImage.sprite = stateInfo.sprite;
      itemHeaderText.SetText(stateInfo.name);
      itemDescriptionText.SetText(stateInfo.description);
      itemDescriptionContainer.SetActive(true);
   }
}
