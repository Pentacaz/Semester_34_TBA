using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Slider timerSlider;
    public float sliderTimer;
    public bool timerStop;
    public GameObject highlightedArea;
    private NavMeshPatrolBakes navMeshPatrol;
    
    private NpcSpotLocation spotLocation;



     private void Start()
    {
        navMeshPatrol = GetComponent<NavMeshPatrolBakes>();
        spotLocation = FindObjectOfType<NpcSpotLocation>();

    }

     // the timer starts
   public void StartTimer()
    {
        timerStop = false;
        timerSlider.maxValue = sliderTimer;
        timerSlider.value = sliderTimer;
        StartCour();
    }
    
    
    public void StartCour()
    {
        StartCoroutine(StartTimerTicker());
    }

    
    IEnumerator StartTimerTicker()
    {
        while (timerStop == false)
        {
            sliderTimer -= Time.deltaTime;
            yield return new WaitForSeconds(0.001f);

            if (sliderTimer <- 0)
            {
                // if the timer hits 0 The Time stops and the customer leaves the bakery  
                StopTimer(); 
                navMeshPatrol.CompletedOrder();
//                highlightedArea.SetActive(false);
                navMeshPatrol.enabled = false;
                spotLocation.ChangeStatus(false);


            }

            if (timerStop == false)
            {
                timerSlider.value = sliderTimer;
            }
            
        }
    }

    public void StopTimer()
    {
        timerStop = true;

    }

   
}
