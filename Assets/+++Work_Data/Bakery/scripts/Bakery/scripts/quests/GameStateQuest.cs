using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameStateQuest : MonoBehaviour
{
    public static event Action StateChanged;
    // refernece zu idee
    public static event Action <string, int> StateAdded;
    #region Inspector

    [SerializeField] public List<State> states;

    //private InventoryManagerQuest _inventoryManager;

    private RemoveItem removeItem;
    #endregion

    private void Start()
    {
      //  _inventoryManager = FindObjectOfType<InventoryManagerQuest>();
        removeItem = GetComponent<RemoveItem>();
    }

    public State Get(string id)
    {
        foreach (State state in states)
        {
            if (state.id == id)
            {
                return state;
            }
        }
        
        return null;
    }
    
    public void Add(string id, int amount, bool invokeEvent = true)
    {
        print($"ID {id} amount {amount}");
        // Aufbau items
        //Wenn dieses passiert ( item eigesammelt) wir Event StateAdded invoked
        
        
        if (string.IsNullOrWhiteSpace(id))
        {
            Debug.LogError("Id of state is empty. Make sure to give each state an id.", this);
            return;
        }
        
        if (amount == 0)
        {
            Debug.LogWarning($"Trying to add 0 to id '{id}'. This will result in to change to the state.", this);
            return;
        }
        // gibt es das item schon
        State state = Get(id);
        // gibt es nich
        if (state == null)
        {
            State newState = new State(id, amount);
            states.Add(newState);
        }
        else
        {
            state.amount += amount;
        }


        //Event
        if (invokeEvent)
        {
            StateChanged?.Invoke();
            // reference StateMangerItem script
            StateAdded?.Invoke(id,amount);
        }
        


        StartCoroutine(CheckItems());
        

    }
    
    private bool isCoroutineRunning = false;
    IEnumerator CheckItems()
    {
        if (isCoroutineRunning)
        {
            yield break; // Coroutine abbrechen, wenn sie bereits läuft
        }
        isCoroutineRunning = true;
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].amount < 1)
            {
                states.RemoveAt(i);
                i--;
            }
        }
        isCoroutineRunning = false;
       // _inventoryManager.RefreshInventory();
    }


    public void Add(State state, bool invokeEvent = true)
    {
        // state => item ( id amount)
        // durchläuft liste von State s 
        // zerteilt items in bestandteile
        
        Add(state.id, state.amount, invokeEvent);
    }

    public void Remove(State state, bool invokeEvent = true)
    {
        // state => item ( id amount)
        // durchläuft liste von State s 
        
        // zerteilt items in bestandteile
        Add(state.id, -state.amount, invokeEvent);

    }
    
    
    public void Add(List<State> states)
    { // für jeden gegen stand in der liste State wird es aufgeruffen 
        // States = > liste ; zuerst aufgeruffen
        // liste von state s
        foreach (State state in states)
        {
            Add(state, false);
        }
        StateChanged?.Invoke();
        // einmal Event ausgeführt wen Event true 
       
    }

    public List<State> GetStateList()
    { //for Inventory Mangaer 
        return states;
    }
   
    
    /// <summary>
    /// Check <paramref name="conditions"/> against the game <see cref="states"/>. All conditions are implicitly AND connected.
    /// A condition passes if the value of the <see cref="State"/> in the game <see cref="states"/> is equal or higher than the value of the condition.
    /// </summary>
    /// <param name="conditions">List of conditions to check.</param>
    /// <returns>If all <paramref name="conditions"/> passed.</returns>
    public bool CheckConditions(List<State> conditions)
    {
        // Check each condition in the list of conditions.
        foreach (State condition in conditions)
        {
            // Get the state with the same id as the condition.
            State state = Get(condition.id);
            // Extract the value if the state exists; otherwise 0.
            int stateAmount = state != null ? state.amount : 0;
            // Compare the value of the state against the value of the condition.
            if (stateAmount < condition.amount)
            {
                // Return immediately false if any condition fails.
                // We do not need to check any conditions after that (Short-circuit).
                return false;
            }
        }

        // If we passed through the loop without ever returning false,
        // we reach the end of the function and can confidently return true.
        return true;
    }
}
