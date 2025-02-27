using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcAreaBehaviour : MonoBehaviour
{
    [SerializeField] private string tagName;
    [SerializeField] private List<NpcSpotLocation> npcSpotLocations;
    private NavMeshAgent navMeshAgent;
    
    // if an Npcs enters this area they go to one of the assigned NpcSpotLocations
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagName))
        {
            for (int i = 0; i < npcSpotLocations.Count; i++)
            {
                if (!npcSpotLocations[i].isOccupied)
                {
                    npcSpotLocations[i].ChangeStatus(true);
                    navMeshAgent = other.GetComponent<NavMeshAgent>();
                    NavMeshPatrolBakes navMeshPatrolBakes = other.GetComponent<NavMeshPatrolBakes>();

                    navMeshPatrolBakes.SetNavMeshDestination(npcSpotLocations[i]);
                    
                    if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance + 0.01f)
                    {
                        navMeshAgent.SetDestination(npcSpotLocations[i].location.position);
                    }
                    break;
                }
             
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

    //UnOccupies all the NpcSpotLocations
    public void NotOccupied()
    {
        for (int i = 0; i < npcSpotLocations.Count; i++)
        {
            npcSpotLocations[i].isOccupied = false;
        }
    }
    
}