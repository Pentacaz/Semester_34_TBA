using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class StateManagerItem : MonoBehaviour
{/// <summary>
 /// For items
 /// Array
 /// 
 /// </summary>
    // was für items ich einsammle 

    
    // eine liste vom typ X mit dem namen y
    //private List <State> statesInfos;
    //Array
    // festgelegte Größe 
    [SerializeField] private StateInfo[] _stateInfos;

    [SerializeField] private GameObject stateContainer;
    [SerializeField] private Image _image;

    [SerializeField] private Button closeButton;
    
    //referenec to GameController Script (for stop player)
    private GameController _controller;
    //private PlayerController _player;

   

   // public static event Action<> DialogeContinue;


   private void Awake()
   {
       // stop player rference / finden
       _controller = FindObjectOfType<GameController>();
   }

   private void OnEnable()
    {   // an Script
        // wen item eeigesammelt/ eigefügt passiert NewStateCollected()
        //Subscriped von GameState
        GameState.StateAdded += NewStateCollected;
    }

    private void OnDisable()
    {    // aus Script
        // unsupscriped
        GameState.StateAdded -= NewStateCollected;
    }

   
    void NewStateCollected ( string id , int amount)
    {
    }
    

    public StateInfo GetStateInfoById(string id)
    {
        foreach (StateInfo statetinfo in _stateInfos)
        {
            // nach id fragen 
            if (statetinfo.id == id)
            {
                return statetinfo;
            }
        }

        return null;
    }
    
    
    
}

