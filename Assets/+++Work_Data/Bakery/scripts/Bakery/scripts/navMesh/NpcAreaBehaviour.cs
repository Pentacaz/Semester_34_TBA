using System;
using System.Collections.Generic;
using UnityEngine;

public class NpcAreaBehaviour : MonoBehaviour
{
    [SerializeField] private List<NpcSpotLocation> npcSpotLocations;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC_ActionPatrol"))
        {
            for (int i = 0; i < npcSpotLocations.Count; i++)
            {
                if (!npcSpotLocations[i].isOccupied)
                {
                    npcSpotLocations[i].ChangeStatus(true);
                    //other.GetComponent<NavMeshPatrol>().SetNavMeshDestination(npcSpotLocations[i]);
                    break;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC_ActionPatrol"))
        {
           // other.GetComponent<NavMeshPatrol>().CheckForNpcSpotLocation();
        }
    }
}