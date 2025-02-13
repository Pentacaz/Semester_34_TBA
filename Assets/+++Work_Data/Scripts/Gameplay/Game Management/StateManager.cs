using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    private GameController gameController;
    
    // Einen Array vom Typ X mit dem Namen Y
    [SerializeField] private StateInfo[] _stateInfos;
    
    [FormerlySerializedAs("item_PanelContainer")] [SerializeField] private GameObject state_PanelContainer;
    [SerializeField] private Image item_image;
    [SerializeField] private TextMeshProUGUI text_itemHeader;
    [SerializeField] private TextMeshProUGUI text_itemDescription;
    [SerializeField] private Button itemButton;
    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void OnEnable()
    {
        GameState.StateAdded += AddNewState;
    }

    private void OnDisable()
    {
        GameState.StateAdded -= AddNewState;
    }

    void AddNewState(string id, int amount)
    {
        foreach (StateInfo stateInfo in _stateInfos)
        {
            if (id == stateInfo.id)
            {
                item_image.sprite = stateInfo.sprite;
                text_itemHeader.SetText(stateInfo.name);
                text_itemDescription.SetText(stateInfo.description);

                StartCoroutine(DelayOpenPanel());
            }
        }
        
        
    }

    IEnumerator DelayOpenPanel()
    {
        yield return null;
        state_PanelContainer.SetActive(true);
        gameController.StartStatePopUp();
        
        Selectable newSelection = itemButton;
        
        yield return null; // Wait for next Update() / next frame

        newSelection.Select();
    }

    public void CloseStatePopUp()
    {
        EventSystem.current.SetSelectedGameObject(null);
        state_PanelContainer.SetActive(false);
        //gameController.EndStatePopUp();
    }

    public StateInfo GetStateInfoById(string id)
    {
        foreach (StateInfo stateInfo in _stateInfos)
        {
            if (id == stateInfo.id)
            {
                return stateInfo;
            }
        }

        return null;
    }
}



