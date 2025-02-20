using UnityEngine;
using UnityEngine.Events;

public class RemoveItemQuest : MonoBehaviour
{
    public State state;
    [SerializeField] State money;

    [SerializeField]  private UnityEvent onCollected;
    [SerializeField] private GameObject Indicator;
    private NavMeshPatrolBakes navMeshPatrol;
    
    private Interactable _selectedInteractable;
    private GameStateQuest gameState;
    

    private void Start()
    {
        navMeshPatrol = GetComponent<NavMeshPatrolBakes>();
        gameState = GetComponent<GameStateQuest>();
    }

    public void Remove()
    {
        onCollected.Invoke();
        
        FindObjectOfType<GameStateQuest>().Remove(state);
        //gameState.Get(state.id);

        Indicator.SetActive(false);
        
    }

    public void TakeOrder()
    {
        
        FindObjectOfType<GameStateQuest>().Add(state);

        
    }
    
    public void GiveOrder()
    {

      
            FindObjectOfType<GameStateQuest>().Add(money);
            FindObjectOfType<GameStateQuest>().Remove(state);
        
        

    }
    
    public void Buy()
    {
        Indicator.SetActive(true);
        FindObjectOfType<GameStateQuest>().Remove(state);

        
    }

    public void IndicatorState()
    {
        Destroy(Indicator);
        navMeshPatrol.CompletedOrder();
        
    }

    
}
