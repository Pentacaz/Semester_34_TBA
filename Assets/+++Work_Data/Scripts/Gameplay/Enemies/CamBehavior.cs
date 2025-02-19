using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamBehavior : MonoBehaviour
{
    public Transform cameraTarget; // The target the FreeLook camera is following (e.g., the player)
    public float shakeDuration = 0.5f; // Duration of the shake effect
    public float shakeStrength = 0.1f; // Strength of the shake
    public float baseShakeStrength; 
    private Vector3 originalPosition; // The original position of the target
    private float shakeElapsedTime = 0f;
    public bool isImpactShake = false;
    public float impactShakeValue;
    void Start()
    {
            originalPosition = cameraTarget.localPosition;
            baseShakeStrength = shakeStrength;
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

            // Apply the offset to the target's position
            cameraTarget.localPosition = originalPosition + randomOffset;

            // Reduce the shake timer
            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            // Reset the target's position when the shake is over
            cameraTarget.localPosition = originalPosition;
        }
    }

    public void CamShake()
    {
        // Start the shake effect
        shakeElapsedTime = shakeDuration;
    }
}


