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


 /*   private void Start()
    {
        timerSlider.maxValue = sliderTimer;
        timerSlider.value = sliderTimer;
        StartTimer();
    }
*/

    public void StartTimer()
    {
        StartCoroutine(StartTimerTicker());
          timerSlider.maxValue = sliderTimer;
        timerSlider.value = sliderTimer;
        timerStop = true;
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
            }

            if (timerStop == false)
            {
                timerSlider.value = sliderTimer;
            }
            
        }
    }
}
