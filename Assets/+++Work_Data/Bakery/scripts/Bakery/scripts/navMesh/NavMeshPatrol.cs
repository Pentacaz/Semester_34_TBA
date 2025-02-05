using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NavMeshPatrol : MonoBehaviour
{

   private static readonly int Hash_MovementSpeed = Animator.StringToHash("MovementSpeed");
   
   #region Inspector

   [SerializeField] private Animator anim;


   [Header("Waypoints")]
   [SerializeField] private bool randomOrder;
   [SerializeField] private List<GameObject> waypoints;
   [SerializeField] private bool waitAtWaypoint = true;
   [SerializeField] private Vector2 waitDuration = new Vector2(1, 5);

   [Header("Gizmos")] 
   
   [SerializeField] private bool showWaypoints = true;
   
   
   #endregion


   public NavMeshAgent navMeshAgent;
   private int currentWaypointIndex = -1;
   private bool isWaiting;

   
   #region Unity Event Functtion

 


   private void Awake()
   {
      
      navMeshAgent = GetComponent<NavMeshAgent>();
      
      navMeshAgent.autoBraking = waitAtWaypoint;
      waypoints = new List<GameObject>();
      waypoints.AddRange(GameObject.FindGameObjectsWithTag("wp"));
   }
   
   
   private void Start()
   {
      SetNextWaypoint();
     // StopPatrol();
   }


   private void Update()
   {
      anim.SetFloat(Hash_MovementSpeed, navMeshAgent.velocity.magnitude);
      
      
      if (!navMeshAgent.isStopped)
      {
          CheckIfWaypointIsReached();
      }
   }

   #endregion
   
   
   #region Navigation

   public void StopPatrolForDialogue()
   {
      StopPatrol();
      DialogueController.DialogueClosed += ResumePatrol;
   }

   public void StopPatrol()
   {
      navMeshAgent.isStopped = true;
   }


   public void ResumePatrol()
   {
      navMeshAgent.isStopped = false;
   }



   private void SetNextWaypoint()
   {
      switch (waypoints.Count)
      {
         case 0:

            Debug.LogError("No Waypoints set for NavMesh", this);
            return; 
            
         
         case 1:
            if (randomOrder)
            {
               Debug.LogError("Only one waypoint set for NAvMeshPatrol.Need atleast 2 with randomOrder enabled", this);
               return;
            }
            else
            {
               Debug.LogError("Only one waypoint set for NavMeshPatrol",this);
               return;
            } 
            break;
      }

      if (randomOrder)
      {
         int newWaypointIndex;

         do
         {
            newWaypointIndex = Random.Range(0, waypoints.Count);
         } while (newWaypointIndex == currentWaypointIndex);

         currentWaypointIndex = newWaypointIndex;
      }
      else
      {
         currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
      }

      navMeshAgent.destination = waypoints[currentWaypointIndex].transform.position;

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
      isWaiting = true;
      yield return new WaitForSeconds(duration);
      isWaiting = false;
      
      SetNextWaypoint();
   }
   #endregion


   #region Gizmos

   private void OnDrawGizmos()
   {
      if (!showWaypoints) return;


      for (int i = 0; i < waypoints.Count; i++)
      {
         GameObject waypoint = waypoints[i];
         Gizmos.color = currentWaypointIndex == i ? Color.green : Color.yellow;
         Gizmos.DrawSphere(waypoint.transform.position,0.3f);

         if (!randomOrder)
         {
            Gizmos.DrawLine(i == 0 ? waypoints[^1].transform.position : waypoints[i-1].transform.position,waypoints[i].transform.position);
         }
      }
   }

   #endregion

   public void WaitWhileEAting()
   {
      StopPatrol();
      StartCoroutine(StayAtBakery());
   }

   IEnumerator StayAtBakery()
   {
      yield return new WaitForSeconds(5);
      ResumePatrol();
   }
   
}
