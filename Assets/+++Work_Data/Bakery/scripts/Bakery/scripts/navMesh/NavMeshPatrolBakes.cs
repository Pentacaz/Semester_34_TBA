using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

/// <summary>
/// Gives a <see cref="NavMeshAgent"/> a patrolling behaviour, moving between a set of points.
/// </summary>
public class NavMeshPatrolBakes : MonoBehaviour
{
    /// <summary>Hashed "MovementSpeed" animator parameter for faster access.</summary>
    private static readonly int MovementSpeedId = Animator.StringToHash("MovementSpeed");
    private static readonly int ActionId = Animator.StringToHash("ActionId");
    private static readonly int ActionTrigger = Animator.StringToHash("ActionTrigger");
    private static readonly int ActionEndTrigger = Animator.StringToHash("ActionEndTrigger");
    private static readonly int BehaviourTrigger = Animator.StringToHash("BehaviourTrigger");
    #region Inspector

    [Tooltip("Animator of the character mesh.")]
    [SerializeField] private Animator animator;

    [Header("Waypoints")]

    [Tooltip("The next waypoint is chosen at random.")]
    [SerializeField] private bool randomOrder;

    [Tooltip("List of waypoints for the NavMeshAgent to walk to. Make sure to put at least two waypoints into this list!")]
    [SerializeField] private List<GameObject> waypoints;
    
    [SerializeField] private GameObject bin;

    [Tooltip("Wait a certain amount of time when reaching a waypoint.")]
    [SerializeField] private bool waitAtWaypoint = true;

    [Min(0)]
    [Tooltip("Min/Max wait duration at each waypoint in seconds. WaitAtWaypoint needs to be enabled.")]
    [SerializeField] private Vector2 waitDuration = new Vector2(1, 5);

    [Header("Gizmos")]

    [Tooltip("Show the a debug visualization for the waypoints.")]
    [SerializeField] private bool showWaypoints = true;

    #endregion

    /// <summary>Cached <see cref="NavMeshAgent"/>.</summary>
    public NavMeshAgent navMeshAgent;

    /// <summary>Current index of the waypoint in <see cref="waypoints"/> the <see cref="navMeshAgent"/> tries to move to.</summary>
    private int currentWaypointIndex = -1; // Is -1 on start so it is incremented to 0.

    /// <summary>If currently waiting at a waypoint before moving to the next.</summary>
    private bool isWaiting;

    private bool isInActionArea;
    private bool isFinished;
    public NpcSpotLocation npcSpotLocation;

    
    #region Unity Event Functions

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        // Disable auto breaking if we don't want to wait at each waypoint.
        navMeshAgent.autoBraking = waitAtWaypoint;
        waypoints.AddRange(GameObject.FindGameObjectsWithTag("wp"));
        bin = GameObject.FindWithTag("Bin");;
        
    }

    private void Start()
    {
        // Move to the first waypoint on game start.
        SetNextWaypoint();
        
    }

    private void Update()
    {
        
        // Update the MovementSpeed in the animator with the speed of the navMeshAgent.
        animator.SetFloat(MovementSpeedId, navMeshAgent.velocity.magnitude);

        if (!navMeshAgent.isStopped && !isInActionArea)
        {
            CheckIfWaypointIsReached();
        }
        else if (isInActionArea && !isFinished)
        {
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance + 0.01f)
            {
                navMeshAgent.SetDestination(npcSpotLocation.location.position);
            }
        }
    }
    
    #endregion

    #region Navigation
    /// <summary>
    /// Stop the <see cref="navMeshAgent"/> from moving for an interaction.
    /// Call this if it should automatically resume it's patrol once the dialogue finishes.
    /// </summary>
    public void StopPatrolForDialogue()
    {
        StopPatrol();
        // Subscribe to DialogueClosed to resume patrol.
        //DialogueController.DialogueClosed += ResumePatrol;
    }

    /// <summary>
    /// Stop the <see cref="navMeshAgent"/> from moving.
    /// </summary>
    public void StopPatrol()
    {
        navMeshAgent.isStopped = true;
    }

    /// <summary>
    /// Let the <see cref="navMeshAgent"/> move again.
    /// </summary>
    public void ResumePatrol()
    {
        isInActionArea = false;
        navMeshAgent.isStopped = false;
        // Unsubscribe from DialogueClosed once triggered. Will do nothing if never subscribed.
        //DialogueController.DialogueClosed -= ResumePatrol;
    }

    /// <summary>
    /// Set the destination of the <see cref="navMeshAgent"/> to the next waypoint in <see cref="waypoints"/>.
    /// </summary>
    private void SetNextWaypoint()
    {
        // Safety checks in case too few waypoints are set in the inspector.
        switch (waypoints.Count)
        {
            case 0:
                Debug.LogError("No waypoints set for NavMeshPatrol", this);
                return;
            case 1:
                if (randomOrder)
                {
                    Debug.LogError("Only one waypoint set for NavMeshPatrol. Need at least 2 with randomOrder enabled", this);
                    return;
                }
                else
                {
                    Debug.LogWarning("Only one waypoint set for NavMeshPatrol.", this);
                    break;
                }
        }

        if (randomOrder)
        {
            int newWaypointIndex;
            // Pick a new random waypoint index until it is different from the current one.
            do
            {
                newWaypointIndex = Random.Range(0, waypoints.Count);
            }
            while (newWaypointIndex == currentWaypointIndex);
            currentWaypointIndex = newWaypointIndex;
        }
        else
        {
            // Increase waypoint index and loop around back to 0.
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }

        // Set the destination of the navmesh agent based on the current waypoint index.
        // The navmesh agent automatically tries to reach this destination.
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].transform.position);
        print("Set Next waypoint");
    }

    // completes the order and the customer goes to the bin where it gets deleted
    public void CompletedOrder()
    {
        navMeshAgent.SetDestination(bin.transform.position);
        navMeshAgent.isStopped = false;
        isFinished = true;
        CheckForNpcSpotLocation();

    }
    
    
    /// <summary>
    /// Check if the navmesh agent has reached its destination and if so set the next waypoint.
    /// </summary>
    private void CheckIfWaypointIsReached()
    {
        // Don't check while we are already waiting before the next waypoint.
        if (isWaiting) { return; }

        // Abort if still calculating path to destination.
        if (navMeshAgent.pathPending) { return; }

        // Check if navmesh agent has sufficiently reached its destination.
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 0.01f) // Small addition because of floating point errors.
        {
            if (waitAtWaypoint)
            {
                StartCoroutine(WaitBeforeNextWaypoint(Random.Range(waitDuration.x, waitDuration.y)));
            }
            else
            {
                SetNextWaypoint();
                CompletedOrder();

            }
        }
    }
    
    private IEnumerator WaitBeforeNextWaypoint(float duration)
    {
        isWaiting = true;
        yield return new WaitForSeconds(duration);
        isWaiting = false;
        SetNextWaypoint();
        
    }

    public void SetNavMeshDestination(NpcSpotLocation npcSpotLocation)
    {
        this.npcSpotLocation = npcSpotLocation;
        isInActionArea = true;
        
    }

    public void CheckForNpcSpotLocation()
    {
        if (npcSpotLocation == null) return;
        npcSpotLocation.ChangeStatus(false);
         npcSpotLocation = null;
    }

    #endregion

    #region Animation

    public void CallAnimationAction(int id)
    {
        animator.SetTrigger(ActionTrigger);
        animator.SetInteger(ActionId, id);
    }
    
    public void CallAnimationBehaviour(int id)
    {
        animator.SetTrigger(BehaviourTrigger);
        animator.SetInteger(ActionId, id);
    }
    
    public void CallAnimationEndAction()
    {
        animator.SetTrigger(ActionEndTrigger);
    }

    #endregion

    #region Gizmos
    
    private void OnDrawGizmos()
    {
        // Do nothing if showWaypoints is not enabled.
        if (!showWaypoints) { return; }

        // Loop over the waypoints.
        for (int i = 0; i < waypoints.Count; i++)
        {
            // Get the waypoint through the index.
            GameObject waypoint = waypoints[i];

            // Set the color of the following Gizmos: green if it is the current waypoint; yellow otherwise.
            Gizmos.color = currentWaypointIndex == i ? Color.green : Color.yellow;
            // Draw a Gizmo sphere at the waypoint in the Scene Window.
            Gizmos.DrawSphere(waypoint.transform.position, 0.3f);

            // Only draw lines if not random order.
            if (!randomOrder)
            {
                // Previous waypoint or last waypoint in list for closing of loop.
                Gizmos.DrawLine(i == 0 ? waypoints[^1].transform.position : waypoints[i - 1].transform.position, waypoint.transform.position);
            }
        }
    }

    #endregion
}
