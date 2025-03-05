using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsShowcase : MonoBehaviour
{
    private GameState gameState;
    private StateManager stateManager;
    private StateInfo stateInfo;

    [SerializeField] private InventorySlot[] inventorySlots;
    
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amount;

    private void Awake()
    {
        gameState = FindObjectOfType<GameState>();
        stateManager = FindObjectOfType<StateManager>();
    }

    private void Start()
    {
        SetStateInfo(stateInfo);    
    }

    public void SetStateInfo(StateInfo stateInfo)
    {
        this.stateInfo = stateInfo;
        ShowItem(this.stateInfo);
    }
    public void ShowItem(StateInfo stateInfo)
    {
        itemImage.sprite = stateInfo.sprite;
        amount.SetText(stateInfo.amount.ToString());
        
    }




}
