using UnityEngine;
using UnityEngine.Events;

public class RemoveItem : MonoBehaviour
{
    [SerializeField] State state;
    [SerializeField] State money;

    [SerializeField]  private UnityEvent onCollected;
    [SerializeField] private GameObject Indicator;
    private NavMeshPatrolBakes navMeshPatrol;
    
    private Interactable _selectedInteractable;

    private void Start()
    {
        navMeshPatrol = GetComponent<NavMeshPatrolBakes>();
    }

    public void Remove()
    {
        onCollected.Invoke();
        
        FindObjectOfType<GameState>().Remove(state);
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
}
