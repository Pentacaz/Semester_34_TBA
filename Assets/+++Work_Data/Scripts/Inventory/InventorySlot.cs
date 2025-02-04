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
    [SerializeField] private Toggle inventorySlotToggle;
    [SerializeField] private GameObject inventorySlotBorder;
    [SerializeField] private Image iventorySlotImage;
    [SerializeField] private TextMeshProUGUI inventorySlotAmountText;

    public void SetStateInfo(StateInfo stateInfo)
    {
        this.stateInfo = stateInfo;
        SetVisuals();
    }

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    void SetVisuals()
    {
        iventorySlotImage.sprite = stateInfo.sprite;
        inventorySlotAmountText.SetText(stateInfo.amount.ToString());
        TurnOnOffVisuals(true);
    }

    public void TurnOnOffVisuals(bool value)
    {
        inventorySlotToggle.interactable = value;
        iventorySlotImage.gameObject.SetActive(value);
        inventorySlotAmountText.gameObject.SetActive(value); 
    }

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
            inventoryManager.ShowItemDescription(stateInfo);
        }
    }
}
