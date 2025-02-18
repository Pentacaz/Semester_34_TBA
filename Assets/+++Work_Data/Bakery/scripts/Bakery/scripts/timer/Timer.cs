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
    



     private void Start()
    {
        navMeshPatrol = GetComponent<NavMeshPatrolBakes>();
    }

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
                StopTimer(); 
                navMeshPatrol.CompletedOrder();
                highlightedArea.SetActive(false);
                
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
