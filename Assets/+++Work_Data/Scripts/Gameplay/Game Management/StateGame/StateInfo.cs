using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateInfo
{
    // Monobehaviour erlaubt es auf object zu ziehen
    // serialzable = im system heinzufügen
    
    public string id;
    
    public int amount;

    public string name;

    public string description;

    public Sprite sprite;

 
    public StateInfo(string id, int amount,Sprite sprite,string description)
    {
        this.id = id;
        this.amount = amount;
        this.sprite = sprite;
        this.description = description;
        //this.name = name;
    }
}
