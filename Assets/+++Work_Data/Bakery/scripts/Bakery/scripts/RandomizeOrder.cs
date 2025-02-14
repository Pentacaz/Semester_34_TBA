using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomizeOrder : MonoBehaviour
{
   private StateManager stateManager;
   [SerializeField] private Image image;


   private void Awake()
   {
      stateManager = GetComponent<StateManager>();
      Randomize();
   }

   public void Randomize()
   {
       int statemanager = Random.Range(0, stateManager._stateInfos.Length);
   }
   
   
   
}
