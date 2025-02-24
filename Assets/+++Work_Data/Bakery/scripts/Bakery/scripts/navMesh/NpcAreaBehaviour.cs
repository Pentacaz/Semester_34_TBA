using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcAreaBehaviour : MonoBehaviour
{
    [SerializeField] private string tagName;
    [SerializeField] private List<NpcSpotLocation> npcSpotLocations;
    private NavMeshAgent navMeshAgent;
    private NpcSpotLocation npcSpotLocation;
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagName))
        {

            for (int i = 0; i < npcSpotLocations.Count; i++)
            {
                if (!npcSpotLocations[i].isOccupied)
                {
                    npcSpotLocations[i].ChangeStatus(true);
                    other.GetComponent<NavMeshPatrolBakes>().SetNavMeshDestination(npcSpotLocations[i]);
                    
                    if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance + 0.01f)
                    {
                        navMeshAgent.SetDestination(npcSpotLocation.location.position);

                    }
                    break;
                }
                /*else if(npcSpotLocations[i].isOccupied)
                {
                    npcSpotLocations[i].ChangeStatus(false);

                    other.GetComponent<NavMeshPatrolBakes>().CompletedOrder(); 
                }
                */
                
            }
        }
    }
    /*

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagName))
        {
            other.GetComponent<NavMeshPatrolBakes>().CheckForNpcSpotLocation();
        }
    }
    */
    
    
}