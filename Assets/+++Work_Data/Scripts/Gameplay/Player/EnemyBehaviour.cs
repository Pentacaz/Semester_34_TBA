using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    
    public EnemyInflictor _enemyInflictor;
    
    
    #region Inspector
    //private static readonly int Hash_MovementSpeed = Animator.StringToHash("MovementSpeed");
    [SerializeField] private Animator anim;

    [Header("Waypoints")]
    [SerializeField] private bool randomOrder;
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private List<Transform> playerWaypoints;
    [SerializeField] private bool waitAtWaypoint = true;
    [SerializeField] private Vector2 waitDuration = new Vector2(1, 5);

    [Header("Gizmos")] [SerializeField] private bool showWaypoints = true;
    [SerializeField] private bool showRange= true;

    #endregion
    #region Chase NAV Mesh
    public bool _playerInRange;
    public Transform player;
    public Transform npc;
    [SerializeField] private  Vector3 playerPosition;
    [SerializeField] private  Vector3 npcPosition;
    [SerializeField] private float playerdmgArea;
    private NavMeshAgent navMeshAgent;
    private int currentWaypointIndex = -1;
    private bool isWaiting;
    #endregion

    public GameObject dmgObject;
    #region MyRegion

    public List<GameObject> projectiles;

    #endregion

    private EnemyStatus _enemyStatus;
    [SerializeField] private EnemyType _enemyType;

  
   public enum EnemyType
    {
      GROUND,
      RANGE
    }

   
    #region Unity Event Functions

    private void Awake()
    {
     
        navMeshAgent.autoBraking = waitAtWaypoint;
    }

    private void Start()
    {
        SetNextWaypoint();
       // navMeshAgent.speed = _enemyStatus.enemySpeed;
        navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyStatus = GetComponent<EnemyStatus>();
    }

    private void Update()
    {
        //anim.SetFloat(Hash_MovementSpeed, navMeshAgent.velocity.magnitude);
        
        if (!navMeshAgent.isStopped)
        {
             CheckIfWaypointIsReached();
        }

        playerPosition = player.position;
        npcPosition = npc.position;
      

      CalculatePlayerAttackRange(playerdmgArea);

    }

    #endregion

    #region Navigation

    public void StopPatrol()
    {
        navMeshAgent.isStopped = true;
    }

    public void ResumePatrol()
    {
        navMeshAgent.isStopped = false;
        DialogueController.DialogueClosed -= ResumePatrol;
    }

    private void SetNextWaypoint()
    {
        if (_playerInRange)
        {
            navMeshAgent.destination = player.position;

        }

     
        switch (waypoints.Count)
        {
            case 0:
                Debug.LogError("No waypoints set for NavMesh", this);
                return;

            case 1:
                Debug.LogError(randomOrder 
                    ? "Only one waypoint set for NavMeshPatrol. Need at least 2 with randomOrder enabled" 
                    : "Only one waypoint set for NavMeshPatrol", this);
                return;
        }

        currentWaypointIndex = randomOrder 
            ? GetRandomWaypointIndex() 
            : (currentWaypointIndex + 1) % waypoints.Count;

        navMeshAgent.destination = waypoints[currentWaypointIndex].position;
    }

    private int GetRandomWaypointIndex()
    {
        int newIndex;
        do
        {
            newIndex = Random.Range(0, waypoints.Count);
        } while (newIndex == currentWaypointIndex);
    
        return newIndex;
    }
    

    private void CheckIfWaypointIsReached()
    {
        if (isWaiting)
        {
            return;
        }

        if (navMeshAgent.pathPending)
        {
            return;
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 0.01f)
        {
            if (waitAtWaypoint)
            {
                StartCoroutine(WaitBeforeNextWaypoint(Random.Range(waitDuration.x, waitDuration.y)));
            }
            else
            {
                SetNextWaypoint();
            }
        }
        
        
    }

    public void ChasePlayer(bool val)
    {
        _playerInRange = val;
    }
    
    public void CalculatePlayerAttackRange(float playerDamageRange)
    {
        float distance = Vector3.Distance (npc.position, player.position);
        if (distance < playerDamageRange)
        {
            EnemyAttack();
            _playerInRange = true;
            ChangeStance();
        }
        else
        {
            _playerInRange = false;
        }
            
    }
    
    
    public void ChangeStance()
    {
       
        Vector3 delta = new Vector3(playerPosition.x - npcPosition.x, 0.0f, playerPosition.z - npcPosition.z);
        Quaternion rotation = Quaternion.LookRotation(delta);

        npc.rotation = rotation;
    }
    

    IEnumerator WaitBeforeNextWaypoint(float duration)
    {
        isWaiting = true;
        yield return new WaitForSeconds(duration);
        isWaiting = false;
        
        SetNextWaypoint();
    }

#endregion


public void EnemyAttack()
{
        switch (_enemyType)
        {
            case EnemyType.RANGE :
                
                Debug.Log("in range of ranged enemy");   
                break;
            case EnemyType.GROUND :
               
            
                
                Debug.Log("in range of grounded enemy");   
                break;
              
        }
    
}

//come back to this later - after scriptable objects are set up
public void ChaseBehavior()
{
    float timer = 3f;
    
    
    if (_enemyInflictor.canAttack && _playerInRange)
    {
        dmgObject.SetActive(true);
    }
    else
    {
        dmgObject.SetActive(false);
    }
}
public void RangedProjectile()
{
    
    

}

    #region Gizmos

    private void OnDrawGizmos()
    {
        if(!showWaypoints) return;

        for (int i = 0; i < waypoints.Count; i++)
        {
            Transform waypoint = waypoints[i];
            Gizmos.color = currentWaypointIndex == i ? Color.green : Color.yellow;
            Gizmos.DrawSphere(waypoint.position, 0.3f);

            if (!randomOrder)
            {
                Gizmos.DrawLine(i == 0 ? waypoints[^1].position : waypoints[i-1].position, waypoints[i].position);
            }
        }

        if (showRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(npc.position,playerdmgArea);
        }
    }

    #endregion
}


