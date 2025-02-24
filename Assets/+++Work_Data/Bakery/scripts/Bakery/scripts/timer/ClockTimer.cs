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

    private NpcSpotLocation spotLocation;

    
    [SerializeField] private GameObject[] customers;

    private ClockTimer clockTimer;
    private float spawnInterval;
    private LightingManager lightingManager;
    private NpcAreaBehaviour areaBehaviour;
    private NavMeshPatrolBakes navMeshPatrol;
    

   private void Awake()
   {
       lightingManager = GetComponent<LightingManager>();
       spotLocation = FindObjectOfType<NpcSpotLocation>();
       areaBehaviour = GetComponent<NpcAreaBehaviour>();
       navMeshPatrol = GetComponent<NavMeshPatrolBakes>();
   }

   private void Start()
    {
        highlightedArea.SetActive(true); 
        
    }

   public void StartTimer()
   {
        sliderTimer = maxTime;
        timerStop = false;
        timerSlider.maxValue = sliderTimer;
        timerSlider.value = sliderTimer;

        highlightedArea.SetActive(false);

        

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
                GameObject[] gos = GameObject.FindGameObjectsWithTag("NPC");
                foreach (GameObject go in gos)
                {

                    Destroy(go);

                }

                StopTimer();
              //  spotLocation.ChangeStatus(false);
                navMeshPatrol.CheckForNpcSpotLocation();


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

        CancelInvoke("RandomOrder"); 
        highlightedArea.SetActive(true);

    }

    public void RandomOrder()
    {
        
        int randomIndex = Random.Range(0, customers.Length); 
        Instantiate(customers[randomIndex]); 

        
        spawnInterval = Random.Range(8,10);
        Invoke("RandomOrder", spawnInterval);
        
    }

    
  
   

   
}
