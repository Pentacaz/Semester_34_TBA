using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomizeOrder : MonoBehaviour
{
   private StateManager stateManager;
   [SerializeField] private Image image;


   private void Awake()
   {
      stateManager = GetComponent<StateManager>();
   }

   public void Randomize()
   {

   }
   
   
   
}
