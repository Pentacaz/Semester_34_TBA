using System;
using UnityEngine;

[Serializable]
public class AnimAction
{
    public string actionName;
    public int actionId;
    
    public Vector2 waitTime;
    public bool isLooping;
    
    public float waitTimeEnd;
}
