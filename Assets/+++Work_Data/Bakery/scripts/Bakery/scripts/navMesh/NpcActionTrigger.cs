using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcActionTrigger : MonoBehaviour
{
    [SerializeField] private string tagName;
    [SerializeField] private int possibility;
    [SerializeField] private bool rotateNpc;
    [SerializeField] private float rotateSpeed = 180f;
    [SerializeField] private List<AnimAction> animActions;

    [Header("Editor")] 
    public bool drawGizmos;
    public float arrowLength = 1f;
    public float lineThickness = 3f;
    public Color arrowColor = Color.red;
    
    
    // on a trigger the chosen animation starts playing after it ends the customer starts walking again
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagName))
        {   
            int animationProbability = UnityEngine.Random.Range(0, 100); // 0-99
            if (animationProbability < possibility)
            {
                int listIndex = Random.Range(0, animActions.Count);

                other.GetComponent<NavMeshPatrolBakes>().StopPatrol();
                other.GetComponent<NavMeshPatrolBakes>().CallAnimationAction(animActions[listIndex].actionId);
                
                StartCoroutine(ResumePatrol(other.GetComponent<NavMeshPatrolBakes>(), 
                    Random.Range(animActions[listIndex].waitTime.x, animActions[listIndex].waitTime.y),
                    animActions[listIndex].isLooping, 
                    animActions[listIndex].waitTimeEnd));
                
                if (rotateNpc)
                {
                    StartCoroutine(RotateNpc(other));
                }
            }
        }  
    }

    IEnumerator RotateNpc(Collider other)
    {
        while (Quaternion.Angle(other.transform.rotation, transform.rotation) > 0.1f)
        {
            other.transform.rotation = Quaternion.RotateTowards(
                other.transform.rotation, 
                transform.rotation, 
                Time.deltaTime * rotateSpeed);
            yield return null;
        }
    }

    IEnumerator ResumePatrol(NavMeshPatrolBakes navMeshPatrol, float waitTime, bool isLooping, float waitEndTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        if(isLooping)
           // navMeshPatrol.CallAnimationEndAction();
        
        yield return new WaitForSeconds(waitEndTime);
        navMeshPatrol.ResumePatrol();
    }
    
    
    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Handles.color = arrowColor;

            Vector3 start = transform.position;

            Vector3 end = start + transform.forward * arrowLength;

            Handles.DrawAAPolyLine(lineThickness, start, end);

            float arrowHeadAngle = 25f;
            float arrowHeadLenght = 0.5f;

            Vector3 left = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(0, arrowHeadAngle, 0) *
                           Vector3.back;
            Handles.DrawAAPolyLine(lineThickness, end, end + left * arrowHeadLenght);

            Vector3 right = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(0, -arrowHeadAngle, 0) *
                            Vector3.back;
            Handles.DrawAAPolyLine(lineThickness, end, end + right * arrowHeadLenght);
        }
    }
}