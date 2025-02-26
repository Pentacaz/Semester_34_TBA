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

    // a new item gets added 
    void AddNewState(string id, int amount)
    {
       
        
        StartCoroutine(DelayOpenPanel());

    }

    IEnumerator DelayOpenPanel()
    {
        yield return null;
        Selectable newSelection;
        newSelection = itemButton;
        
        
        yield return null; // Wait for next Update() / next frame

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



