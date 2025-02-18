using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnEnemies : MonoBehaviour
{
    public List<Vector3> spawnPositions;
    public List<GameObject> enemyPrefabs; 
    public List<GameObject> activeEnemies; 
    public int numberOfEnemiesToSpawn = 5; 
    public int totalRounds = 3; 
    public int currentRound = 1; 
    public bool roundIsActive;
    private List<GameObject> allEnemies; 
    private Collider triggerCollider;
    public bool hasEntered;

    public float coolDownTimer;
    public float cooldowntimervalue;
    

    private void Start()
    {
        AddToEnemiesList();
        cooldowntimervalue = coolDownTimer;
    }

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>(); 
    }

    private void Update()
    {
        Rounds();
        if (hasEntered)
        {
            StartNewRound();
        }

      
       
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player") && !roundIsActive && currentRound <= totalRounds)
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
            Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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
        Debug.Log("All rounds completed! Game Over.");
    }
}
    

