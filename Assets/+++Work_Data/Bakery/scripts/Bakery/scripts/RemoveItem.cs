using UnityEngine;
using UnityEngine.Events;

public class RemoveItem : MonoBehaviour
{
    public State state;
    public State money;

    [SerializeField]  private UnityEvent onCollected;
    [SerializeField] private GameObject Indicator;
    private NavMeshPatrolBakes navMeshPatrol;
    
    private Interactable _selectedInteractable;
    private GameState gameState;
    private Cooking cooking;
    
    
    // this script removes and adds different items. Depending on the need of the customer or what you buy at the shop 


    private void Start()
    {
        navMeshPatrol = GetComponentInParent<NavMeshPatrolBakes>();
        gameState = GetComponent<GameState>();
        cooking = GetComponent<Cooking>();
    }

    public void Remove()
    {
        onCollected.Invoke();
        
        FindObjectOfType<GameState>().Remove(state);

        Indicator.SetActive(false);
        
    }

    public void Cook()
    {
        if (cooking.inRange)
        {
            FindObjectOfType<GameState>().Add(state);
            FindObjectOfType<GameState>().Remove(money);
        }
        else
        {
            FindObjectOfType<GameState>().Remove(money);
        }
    }

    

    public void TakeOrder()
    {
        
        FindObjectOfType<GameState>().Add(state);

        
    }
    
    public void GiveOrder()
    {

      
            FindObjectOfType<GameState>().Add(money);
            FindObjectOfType<GameState>().Remove(state);
        
        

    }
    
    public void Buy()
    {
        Indicator.SetActive(true);
        FindObjectOfType<GameState>().Remove(state);

        
    }

    public void IndicatorState()
    {
        Destroy(Indicator);
        navMeshPatrol.CompletedOrder();
        
    }

    public State Get(string id)
    {
        foreach (State state in gameState.states)
        {
            if (state.id == id)
            {
                return state;
            }
        }
        
        return null;
    }
}
