using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ClockTimer : MonoBehaviour
{

    public Slider timerSlider;
    public float sliderTimer;
    public float maxTime;
    public bool timerStop;
    public GameObject highlightedArea;

    private NpcSpotLocation[] spotLocation;

    
    [SerializeField] private GameObject[] customers;

    private ClockTimer clockTimer;
    private float spawnInterval;
    public LightingManager lightingManager;
    public NpcAreaBehaviour areaBehaviour;
    private NavMeshPatrolBakes navMeshPatrol;
    

    
    
   private void Awake()
   {
       spotLocation = FindObjectsOfType<NpcSpotLocation>();
       navMeshPatrol = GetComponent<NavMeshPatrolBakes>();
       
   }

   private void Start()
    {
        highlightedArea.SetActive(true); 
        
    }
    
   // the timer starts 
   public void StartTimer()
   {
        sliderTimer = maxTime;
        timerStop = false;
        timerSlider.maxValue = sliderTimer;
        timerSlider.value = sliderTimer;

        highlightedArea.SetActive(false);

        
         // customers spawn in at random
        StartCoroutine(StartTimerTicker());
        RandomOrder();
    }
    
    
    

    IEnumerator StartTimerTicker()
    {
        while (timerStop == false)
        {
            sliderTimer -= Time.deltaTime;
            yield return new WaitForSeconds(0.001f);

            if (sliderTimer <- 0)
            {
                //if the timer is 0 or below 0 it disables the lightningmanager -> the day stops
                lightingManager.enabled = false;
                // all the Npcs get deleted 

                GameObject[] gos = GameObject.FindGameObjectsWithTag("NPC");
                foreach (GameObject go in gos)
                {

                    Destroy(go);

                }

                StopTimer();
               // navMeshPatrol.CheckForNpcSpotLocation();
               areaBehaviour.NotOccupied();

            }
            if (timerStop == false)
            {
                timerSlider.value = sliderTimer;
                
            }
            
        }
    }

    // the timer Stops
    public void StopTimer()
    {
        timerStop = true;

        CancelInvoke("RandomOrder"); 
        highlightedArea.SetActive(true);

    }
     
    // customer spawn at random every 8 to 10 seconds
    public void RandomOrder()
    {
        
        int randomIndex = Random.Range(0, customers.Length); 
        Instantiate(customers[randomIndex]); 

        
        spawnInterval = Random.Range(8,10);
        Invoke("RandomOrder", spawnInterval);
        
    }

    
  
   

   
}
