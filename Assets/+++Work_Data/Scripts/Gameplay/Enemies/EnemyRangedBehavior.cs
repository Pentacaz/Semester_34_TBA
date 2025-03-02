using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class EnemyRangedBehavior : MonoBehaviour
{
   
  
    public EnemyState enemyState;
    public EnemyInflictor _enemyInflictor;


    #region Inspector

    private static readonly int Hash_MovementSpeed = Animator.StringToHash("MovementSpeed");
    [SerializeField] private Animator anim;

    [Header("Waypoints")] 
    [SerializeField] private bool randomOrder;
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private List<Transform> playerWaypoints;
    [SerializeField] private bool waitAtWaypoint = true;
    [SerializeField] private Vector2 waitDuration = new Vector2(1, 5);

    #endregion

    #region Chase NAV Mesh
    [Header("Enemy Behaviour")]
    public bool _playerInRange = false;
    public bool rotateWhileAttacking;
    public bool canAttackPlayer = false;
    public GameObject auraDmgObject;
    public GameObject playerHitAreaIndicator;
    public float circleRadius;
    public float _attackCooldownvalue;
    public float auraDmgTime;
    public float chaseTime;
    public bool lockedIn;

    public VisualEffect attackVFX;
    
    [SerializeField] private float enemyAggroArea;
    [SerializeField] private float enemyAttackArea;
    [SerializeField] private float enemyStopArea;
    private CamBehavior _camBehavior;
    #endregion
    [Header("Gizmos")] [SerializeField] private bool showWaypoints = true;
    [SerializeField] private bool showRange = true;

    private EnemyStatus _enemyStatus;
    private EnemyReciever _enemyReciever;
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private int currentWaypointIndex = -1;
    private bool isWaiting;
    private bool _isAttacking = true;
    private float _auraDmgTimer;
    private float _chaseTimer;
    public GameObject alertSymbol;
    #region Unity Event Functions

    private void Awake()
    {
        _camBehavior = GetComponent<CamBehavior>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyStatus = GetComponent<EnemyStatus>();
        _enemyReciever = GetComponent<EnemyReciever>();
        player = FindObjectOfType<PlayerBaseController>().transform;
    }

    private void Start()
    {
        SetNextWaypoint();
        _auraDmgTimer = auraDmgTime;
        _chaseTimer = chaseTime;
        navMeshAgent.speed = _enemyStatus.enemySpeed;
        //_attackCooldownvalue = 0;
        
    }

    private void Update()
    {
        if (!lockedIn)
        {
            playerHitAreaIndicator.transform.position = player.transform.position + new Vector3(0, 0.2f, 0);
        }
      
        
        CalculateEnemyAggroRange(enemyAggroArea);
        CalculateEnemyAttackRange(enemyAttackArea);
        AttackCooldown();
        IsChasing();
        //UpdateAnimator()
        anim.SetFloat(Hash_MovementSpeed, navMeshAgent.velocity.magnitude);
        
        
        
        if (!navMeshAgent.isStopped && 
            (enemyState == EnemyState.PATROL || enemyState == EnemyState.IDLE))
        {
            CheckIfWaypointIsReached();
        }
        
        if ((enemyState != EnemyState.PATROL || enemyState != EnemyState.IDLE) && !navMeshAgent.isStopped)
        {
            //navMeshAgent.destination = player.position;
        }
        CalculateEnemyStopRange(enemyStopArea);
    }

    #endregion

    #region Navigation

    public void StopPatrol()
    {
        navMeshAgent.ResetPath();
        navMeshAgent.isStopped = true;
    }

    public void ResumePatrol()
    {
        navMeshAgent.isStopped = false;
        DialogueController.DialogueClosed -= ResumePatrol;
    }

    private void SetNextWaypoint()
    {
        switch (waypoints.Count)
        {
            case 0:
                //Debug.LogError("No waypoints set for NavMesh", this);
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
        enemyState = EnemyState.PATROL;
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
    
    IEnumerator WaitBeforeNextWaypoint(float duration)
    {
        enemyState = EnemyState.IDLE;
        isWaiting = true;
        yield return new WaitForSeconds(duration);
        isWaiting = false;

        SetNextWaypoint();
    }
    #endregion

    #region Enemy Behaviour

    public void CalculateEnemyAggroRange(float aggroRange)
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < aggroRange)
        {
            print("aggro");
      
    
            if (enemyState != EnemyState.CHASE &&
                enemyState != EnemyState.CANATTACK &&
                enemyState != EnemyState.ATTACKING)
            {
                enemyState = EnemyState.AGGRO;
            }
            navMeshAgent.destination = player.position;

            ChangeStance();
        }
        else if (_playerInRange)
        {
            _playerInRange = false;
       
            if (enemyState != EnemyState.CANATTACK &&
                enemyState != EnemyState.ATTACKING)
            {
                enemyState = EnemyState.CHASE;
                playerHitAreaIndicator.SetActive(false);
            }
        }
    }
    
    public void CalculateEnemyAttackRange(float attackRange)
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < attackRange)
        {
            if (enemyState != EnemyState.ATTACKING)
            {
                canAttackPlayer = true;
                enemyState = EnemyState.CANATTACK;
            }
        }
        else if (canAttackPlayer)
        {
            canAttackPlayer = false;
            enemyState = _playerInRange ? EnemyState.AGGRO : EnemyState.CHASE;
        }
    }
    
    public void CalculateEnemyStopRange(float stopRange)
    {
        float distance = Vector3.Distance(transform.position, player.position);
        //print($"{distance} : {stopRange} : {navMeshAgent.isStopped}");
        if (distance < stopRange)
        {
            navMeshAgent.velocity = Vector3.zero;
            StopPatrol();
        }
        else if(distance > stopRange)
        {
            navMeshAgent.isStopped = false;
        }
    }
    
    void ChangeStance()
    {
        if (enemyState == EnemyState.ATTACKING && !rotateWhileAttacking) return;
        
        Vector3 delta = new Vector3(player.position.x - transform.position.x, 0.0f, player.position.z - transform.position.z);
        Quaternion rotation = Quaternion.LookRotation(delta);
        transform.rotation = rotation;
    }
    
    void AttackCooldown()
    {
        if (enemyState == EnemyState.CANATTACK || _playerInRange && enemyState == EnemyState.CHASE)
        {   
            _attackCooldownvalue -= Time.deltaTime;
            
            if (_attackCooldownvalue <= _enemyStatus.enemyAttackCooldown/2)
            {
                //print($"{_attackCooldownvalue}");
                playerHitAreaIndicator.SetActive(true);
            }  else 
            {
                playerHitAreaIndicator.SetActive(false);
            }

            if (_attackCooldownvalue <= _enemyStatus.enemyAttackCooldown / 4)
            {
                lockedIn = true;
            }
            else
            {
                lockedIn = false;
            }
          
            
            
            if (_attackCooldownvalue <= 0)
            {
                
                _isAttacking = true;
                _attackCooldownvalue = _enemyStatus.enemyAttackCooldown;
                EnemyAttack();
                enemyState = EnemyState.ATTACKING;
            }
        }

        if (_isAttacking)
        {
            _auraDmgTimer -= Time.deltaTime;
            if (_auraDmgTimer <= 0)
            {
                _isAttacking = false;
                _auraDmgTimer = auraDmgTime;
                StopEnemyAttack();
                if (canAttackPlayer)
                {
                    enemyState = EnemyState.CANATTACK;
                } 
                else
                {
                    enemyState = _playerInRange ? EnemyState.AGGRO : EnemyState.CHASE;
                }
            }
        }
    }

    void IsChasing()
    {
        if (enemyState == EnemyState.CHASE)
        {
            _chaseTimer -= Time.deltaTime;
            if (_chaseTimer <= 0)
            {
                _attackCooldownvalue = 0;
                
                enemyState = EnemyState.IDLE;
                _chaseTimer = chaseTime; 
            }
        }
    }
    
    public void EnemyAttack()
    {
        _camBehavior.isImpactShake = true;
        _camBehavior.CamShake();
        SpawnProjectileOnLastPlayerPosition();
        _isAttacking = true;
    }
    public void StopEnemyAttack()
    {
        _camBehavior.isImpactShake = false;
        auraDmgObject.SetActive(false);
        _isAttacking = false;
    }
    

    void SpawnProjectileOnLastPlayerPosition()
    {

        Vector3 spawnPosition = playerHitAreaIndicator.transform.position;

        auraDmgObject.transform.position = spawnPosition;
        auraDmgObject.SetActive(true);
        attackVFX.Play();
    }
    #endregion
    
    #region Gizmos

    private void OnDrawGizmos()
    {
        if (!showWaypoints) return;

        for (int i = 0; i < waypoints.Count; i++)
        {
            Transform waypoint = waypoints[i];
            Gizmos.color = currentWaypointIndex == i ? Color.green : Color.yellow;
            Gizmos.DrawSphere(waypoint.position, 0.3f);

            if (!randomOrder)
            {
                Gizmos.DrawLine(i == 0 ? waypoints[^1].position : waypoints[i - 1].position, waypoints[i].position);
            }
        }

        if (showRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyAggroArea);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, enemyAttackArea);
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, enemyStopArea);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position,circleRadius);
        }
    }

    
    
    #endregion
    
}
