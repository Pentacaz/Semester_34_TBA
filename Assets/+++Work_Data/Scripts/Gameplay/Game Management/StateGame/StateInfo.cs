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

    private void Awake()
    {
        amount = Mathf.Clamp(0,0, 100000);
    }
}
