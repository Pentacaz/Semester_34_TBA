 using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StateManagerQuest : MonoBehaviour
{
    private GameController gameController;
    
    // Einen Array vom Typ X mit dem Namen Y
    [SerializeField] public StateInfo[] _stateInfos;
    
    [SerializeField] private GameObject state_PanelContainer;
  
    [SerializeField] private Button itemButton;
    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void OnEnable()
    {
        GameStateQuest.StateAdded += AddNewState;
    }

    private void OnDisable()
    {
        GameStateQuest.StateAdded -= AddNewState;
    }

    void AddNewState(string id, int amount)
    {
        /*
        foreach (StateInfo stateInfo in _stateInfos)
        {
            if (stateInfo.id == id)
            {
                item_image.sprite = stateInfo.sprite;
                text_itemHeader.SetText(stateInfo.name);
                text_itemDescription.SetText(stateInfo.description);

            }
        }
        */
        
        StartCoroutine(DelayOpenPanel());

    }

    IEnumerator DelayOpenPanel()
    {
        yield return null;
//        state_PanelContainer.SetActive(true);
        Selectable newSelection;
        newSelection = itemButton;
        //gameController.StartStatePopUp();
        
        
        yield return null; // Wait for next Update() / next frame

      //  newSelection.Select();
    }

   

    public StateInfo GetStateInfoById(string id)
    {
        foreach (StateInfo stateInfo in _stateInfos)
        {
            if (stateInfo.id == id)
            {
                return stateInfo;
            }
        }

        return null;
    }
}



