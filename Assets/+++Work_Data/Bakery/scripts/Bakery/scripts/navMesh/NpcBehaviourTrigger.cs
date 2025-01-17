using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class NpcBehaviourTrigger : MonoBehaviour
{
    [SerializeField] private int possibility;

    [SerializeField] private List<AnimAction> animActions;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC_ActionPatrol"))
        {   
            int animationProbability = UnityEngine.Random.Range(0, 100);
            if (animationProbability < possibility)
            {
                int listIndex = Random.Range(0, animActions.Count);

                other.GetComponent<NavMeshPatrol>().StopPatrol();
               // other.GetComponent<NavMeshPatrol>().CallAnimationBehaviour(animActions[listIndex].actionId);
                StartCoroutine(ResumePatrol(other.GetComponent<NavMeshPatrol>(), 
                    Random.Range(animActions[listIndex].waitTime.x,
                        animActions[listIndex].waitTime.y),animActions[listIndex].isLooping, 
                    animActions[listIndex].waitTimeEnd));
            }
        }  
    }

    IEnumerator ResumePatrol(NavMeshPatrol navMeshPatrol, float waitTime, bool isLooping, float waitEndTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        if(isLooping)
        //    navMeshPatrol.CallAnimationEndAction();
        
        yield return new WaitForSeconds(waitEndTime);
        navMeshPatrol.ResumePatrol();
    }
}

