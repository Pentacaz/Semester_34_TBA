using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RemoveItem : MonoBehaviour
{
    [SerializeField] State state;
    [SerializeField]  private UnityEvent onCollected;
    [SerializeField] private GameObject Indicator;
    


    public void Remove()
    {
        onCollected.Invoke();
        FindObjectOfType<GameState>().Remove(state);
        
    }
}
