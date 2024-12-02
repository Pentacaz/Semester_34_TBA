using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Customizer : MonoBehaviour
{

    private CustomizeManager _customizeManager;
    public Animator outfitPartPlayer;
    //#TODO change to appropriate parameters / variables for 3d Mesh.
    public List<RuntimeAnimatorController> optionsPlayer = new List<RuntimeAnimatorController>();
   
    private int currentOption = 0;
  


    private void Awake()
    {
        _customizeManager = Component.FindObjectOfType<CustomizeManager>();
    }
    

    private void Update()
    {
            SyncCharacter(_customizeManager.playerUIGameObject,_customizeManager.player);
    }
    

    public void NextOption()
    {
            currentOption++;
            if ( currentOption >= optionsPlayer.Count)
            {
                currentOption = 0;
            }

            outfitPartPlayer.runtimeAnimatorController = optionsPlayer[currentOption];
    } 

    public void PreviousOption()
    {
            currentOption--;
            if (currentOption <= 0)
            {
                currentOption = optionsPlayer.Count - 1;
            }
            outfitPartPlayer.runtimeAnimatorController = optionsPlayer[currentOption];
        
    }

    void SyncCharacter(GameObject temp, GameObject original)
    {
        //#TODO change to appropriate parameters / variables for 3d Mesh.
        SpriteRenderer[] tempRenderers = temp.GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer[] originalRenderers = original.GetComponentsInChildren<SpriteRenderer>();
        
        for (int i = 0; i < tempRenderers.Length; i++)
        {
            tempRenderers[i].sprite = originalRenderers[i].sprite;
        }
        
    }
}
