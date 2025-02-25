using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnEnemies : MonoBehaviour
{
    #region enemy spawn

    public List<Transform> spawnPositions;
    public List<GameObject> enemyPrefabs; 
    public List<GameObject> activeEnemies; 
    public int numberOfEnemiesToSpawn = 5; 
    public int totalRounds = 3; 
    public int currentRound = 1; 
    public bool roundIsActive;
    public bool noMoreEnemies;
    public List<GameObject> allEnemies; 
    private Collider triggerCollider;
    public bool hasEntered;
    public bool isBoss = false;
    public float coolDownTimer;
    public float cooldowntimervalue;

    private DungeonRoomTracker _dungeonRoomTracker;
    #endregion

    private UiManager _uiManager;
    
    private void Awake()
    {   _uiManager = FindObjectOfType<UiManager>();
        triggerCollider = GetComponent<Collider>();
        _dungeonRoomTracker = FindObjectOfType<DungeonRoomTracker>();
    }
    
    private void Start()
    {
     
        AddToEnemiesList();
        //_uiManager.EnemyCountDisplay(activeEnemies.Count,currentRound);
        cooldowntimervalue = coolDownTimer;
    }

  

    private void Update()
    {
        Rounds();
       
        if (hasEntered)
        {
            StartNewRound();
            _uiManager.uiContainer.SetActive(true);
        }

      
        _uiManager.EnemyCountDisplay(allEnemies.Count,totalRounds);
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (!isBoss && other.CompareTag("Player") && !roundIsActive && currentRound <= totalRounds)
        {
            hasEntered = true;
            triggerCollider.enabled = false;
        }
    }

    public void Rounds()
    {
        if (allEnemies.Count == 0 && roundIsActive)
        {
            CoolDownTimer();
        
        }
    }

  
    void AddToEnemiesList()
    {
        EnemyStatus[] enemyStatusArray = FindObjectsOfType<EnemyStatus>();
        allEnemies = new List<GameObject>();

        foreach (EnemyStatus enemyStatus in enemyStatusArray)
        {
            allEnemies.Add(enemyStatus.gameObject);
        }

        Debug.Log("Found " + allEnemies.Count + " enemies with EnemyStatus component.");
    }

   
    public void StartNewRound()
    {
        hasEntered = false;
        if (currentRound <= totalRounds)
        {
            roundIsActive = true;
            Debug.Log("Starting Round " + currentRound);
            PlaceEnemies();
            AddToEnemiesList();
           
        }
    }

 
    public void EndRound()
    {
        roundIsActive = false;
        Debug.Log("Round " + currentRound + " completed!");
        if (currentRound >= totalRounds)
        {
            _uiManager.uiContainer.SetActive(false);
            EndGame();
        }
        else
        {
            currentRound++;
            StartNewRound();
        }
    }

   
    public void PlaceEnemies()
    {
        if (enemyPrefabs.Count == 0 || spawnPositions.Count == 0)
        {
            Debug.LogError("Please assign enemy prefabs and spawn positions in the inspector.");
            return;
        }

        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            Transform spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition.position, Quaternion.identity);
            activeEnemies.Add(newEnemy);
        }

        Debug.Log("Spawned " + numberOfEnemiesToSpawn + " enemies for round " + currentRound);
        return ;
    }

   
    public void RemoveDefeatedEnemy(GameObject enemy)
    {
        if (allEnemies.Contains(enemy))
        {
            allEnemies.Remove(enemy);
            Debug.Log("Removed defeated enemy from the list.");
        }
    }

    public void CoolDownTimer()
    {
        coolDownTimer -= Time.deltaTime;

    if (coolDownTimer <= 0)
    {  
        EndRound();
        coolDownTimer = cooldowntimervalue;
    }  
    
    }
    public void EndGame()
    {
      //remove everthing from list
        noMoreEnemies = true;
        Debug.Log("All rounds completed! Game Over.");
        if (!isBoss)
        {
            _dungeonRoomTracker.AddClearedRoom();
        }
    }
}
    

