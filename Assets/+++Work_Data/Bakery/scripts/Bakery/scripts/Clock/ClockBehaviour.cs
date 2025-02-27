using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockBehaviour : MonoBehaviour
{
    [SerializeField] private Image clock;
    [SerializeField] private float maxtTime = 1f;
    [SerializeField] private float currentTime;

    private void Start()
    {
        maxtTime = currentTime;
        StartCoroutine(StartTimer());

    }

    public void ClockTimer()
    {
        
    }

    public void StartDay()
    {
        
        StartCoroutine(StartTimer());
        
    }

    IEnumerator StartTimer()
    {
        clock.fillAmount -= Time.deltaTime;
        clock.fillAmount = currentTime / maxtTime;
        yield return new WaitForSeconds(0.001f);

       
    }
}
