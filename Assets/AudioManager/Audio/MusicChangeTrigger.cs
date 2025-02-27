using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    
    // if you enter the trigger the area changes 
    [Header("Area")] 
    [SerializeField] private MusicArea area;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
               AudioManager.instance.SetMusicArea(area);
        }
    }
}
