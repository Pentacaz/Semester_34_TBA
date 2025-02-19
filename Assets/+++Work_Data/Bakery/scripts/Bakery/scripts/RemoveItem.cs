using UnityEngine;
using UnityEngine.Events;

public class RemoveItem : MonoBehaviour
{
    public State state;
    [SerializeField] State money;

    [SerializeField]  private UnityEvent onCollected;
    [SerializeField] private GameObject Indicator;
    private NavMeshPatrolBakes navMeshPatrol;
    
    private Interactable _selectedInteractable;
    private GameState gameState;
    

    private void Start()
    {
        navMeshPatrol = GetComponent<NavMeshPatrolBakes>();
        gameState = GetComponent<GameState>();
    }

    public void Remove()
    {
        onCollected.Invoke();
        
        FindObjectOfType<GameState>().Remove(state);
        //gameState.Get(state.id);

        Indicator.SetActive(false);
        
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
