using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AddSounds : MonoBehaviour
{

    // different sound play. depending on which is needed
    public void Door()
    {
       AudioManager.instance.PlayOneShot(FMODEvents.instance.doorOpen, this.transform.position);
    }
    
    public void DoorClose()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.doorClose, this.transform.position);
    }
    
    public void Dishes()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.dishes, this.transform.position);
    }
    
    public void Microwave()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.mircowave, this.transform.position);
    }
    
    public void NpcEating()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.npcEating, this.transform.position);
    }
    
    public void npcWalking()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.npcWalking, this.transform.position);
    }
}
