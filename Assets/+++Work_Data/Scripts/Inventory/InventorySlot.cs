using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private StateInfo stateInfo;
    private InventoryManager inventoryManager;
    [SerializeField] private Image iventorySlotImage;
    [SerializeField] private TextMeshProUGUI inventorySlotAmountText;
    private StateManager stateManager;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }
    
    private void Awake()
    {
        stateManager = FindObjectOfType<StateManager>();
    }
    public void SetStateInfo(StateInfo stateInfo)
    {
        this.stateInfo = stateInfo;
        SetVisuals();
    }

    

    void SetVisuals()
    {
        iventorySlotImage.sprite = stateInfo.sprite;
        inventorySlotAmountText.SetText(stateInfo.amount.ToString());
        TurnOnOffVisuals(true);
    }

    public void TurnOnOffVisuals(bool value)
    {
     //   inventorySlotToggle.interactable = value;
        iventorySlotImage.gameObject.SetActive(value);
        inventorySlotAmountText.gameObject.SetActive(value); 
       // inventorySlotBorder.gameObject.SetActive(false);
    }

    /*
    public void TurnOffBorder()
    {
        StartCoroutine(InitiateTurnOffBorder());
    }

    IEnumerator InitiateTurnOffBorder()
    {
        yield return null;
        inventorySlotBorder.SetActive(false);
    }

    public void ShowDescription()
    {
        if (inventorySlotToggle.isOn)
        {
            //inventoryManager.ShowItemDescription(stateInfo);
        }
    }
    */
}
