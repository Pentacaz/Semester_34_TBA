using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockTimer : MonoBehaviour
{

    public Slider timerSlider;
    public float sliderTimer;
    public float maxTime;
    public bool timerStop;
    public GameObject highlightedArea;
    private NavMeshPatrol navMeshPatrol;

    [SerializeField] private GameObject customers;
    
    

     private void Start()
    {
        navMeshPatrol = GetComponent<NavMeshPatrol>();
    }

   public void StartTimer()
   {
        sliderTimer = maxTime;
        timerStop = false;
        timerSlider.maxValue = sliderTimer;
        timerSlider.value = sliderTimer;
        customers.SetActive(true);
        
        
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
                timerStop = true;
                customers.SetActive(false);
                highlightedArea.SetActive(true);
                navMeshPatrol.StopPatrol();
                
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
