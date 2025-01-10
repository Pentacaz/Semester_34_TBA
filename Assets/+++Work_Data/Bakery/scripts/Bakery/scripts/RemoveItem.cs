using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RemoveItem : MonoBehaviour
{
    [SerializeField] State state;
    [SerializeField] State money;

    [SerializeField]  private UnityEvent onCollected;
    [SerializeField] private GameObject Indicator;
    
    private Interactable _selectedInteractable;

    public void Remove()
    {
        // Mathf.Clamp(state.amount, 0, 100);
        onCollected.Invoke();
        
        FindObjectOfType<GameState>().Remove(state);
        

        Indicator.SetActive(false);
        
    }

    public void AddMoney()
    {
        FindObjectOfType<GameState>().Add(money);
    }
    
    public void Buy()
    {
        Indicator.SetActive(true);

        FindObjectOfType<GameState>().Remove(state);
        
    }

    public void IndicatorState()
    {
        Indicator.SetActive(false);
        Destroy(Indicator);
    }
}
