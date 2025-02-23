using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamBehavior : MonoBehaviour
{
    public Transform cameraTarget; 
    public float shakeDuration = 0.5f;
    public float shakeStrength = 0.1f;
    public float baseShakeStrength; 
    public float baseShakeDuration; 
    private Vector3 originalPosition; 
    private float shakeElapsedTime = 0f;
    public bool isImpactShake = false;
    public float impactShakeValue;
    void Start()
    {
            cameraTarget = GameObject.Find("Camera_Target").transform;
            originalPosition = cameraTarget.localPosition;
            baseShakeStrength = shakeStrength;
            baseShakeDuration = shakeDuration;

    }

    void Update()
    {
        if (isImpactShake)
        {
            shakeStrength = baseShakeStrength * impactShakeValue ;
            
        }
        else
        {
            shakeStrength = baseShakeStrength;
        }
        if (shakeElapsedTime > 0)
        {
            // Generate random offset for the shake
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ) * shakeStrength;

           
            cameraTarget.localPosition = originalPosition + randomOffset;

        
            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
           
            cameraTarget.localPosition = originalPosition;
        }
    }

    public void CamShake()
    {
     
        shakeElapsedTime = shakeDuration;
    }
}


