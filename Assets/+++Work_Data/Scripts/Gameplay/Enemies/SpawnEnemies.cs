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
    public bool roundIsActive = false;
    public bool noMoreEnemies;
    public List<GameObject> allEnemies;
    public Collider triggerCollider;
    public bool hasEntered;
    public bool isBoss = false;
    public float coolDownTimer;
    public float cooldowntimervalue;

    private DungeonRoomTracker _dungeonRoomTracker;

    #endregion

    #region Indicator


    public List<GameObject> ToSetInactiveAtStart;
    public List<GameObject> ToBeSetActiveAtStart;
    public List<GameObject> ToBeSetActivAtEnd;
    public List<GameObject> ToBeSetInactiveAtEnd;

    #endregion

    private UiManager _uiManager;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UiManager>();
        triggerCollider = GetComponent<Collider>();
     
    }

    private void Start()
    {
        AddToEnemiesList();
        cooldowntimervalue = coolDownTimer;
    }

    private void Update()
    {
        _dungeonRoomTracker = FindObjectOfType<DungeonRoomTracker>();
        if (hasEntered && !roundIsActive && currentRound <= totalRounds)
        {
            StartNewRound();

        }

        allEnemies.Remove(null);
        for (int i = allEnemies.Count - 1; i >= 0; i--)
        {
            if (allEnemies[i].GetComponent<EnemyReciever>().IsDead)
            {
                 allEnemies.RemoveAt(i);
            }
        }
        
        if (roundIsActive && allEnemies.Count == 0)
        {
            EndRound();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isBoss && other.CompareTag("Player") && !roundIsActive && currentRound <= totalRounds)
        {
            hasEntered = true;
            SetActiveStart();
            SetInactiveStart();
            triggerCollider.enabled = false;
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
        roundIsActive = true;
        Debug.Log("Starting Round " + currentRound);
        PlaceEnemies();
        AddToEnemiesList();


    }

    public void EndRound()
    {
        roundIsActive = false;
        Debug.Log("Round " + currentRound + " completed!");
        if (currentRound >= totalRounds)
        {
            FinalRoundOver();
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
            allEnemies.Add(newEnemy);
        }

        Debug.Log("Spawned " + numberOfEnemiesToSpawn + " enemies for round " + currentRound);
    }

    public void FinalRoundOver()
    {
        _dungeonRoomTracker.AddClearedRoom();
        noMoreEnemies = true;
        allEnemies.Clear();
        activeEnemies.Clear();
       SetActiveEnd();
       SetInactiveEnd();
        Debug.Log("All rounds completed!");
     
          
        
    }

    public void SetActiveStart()
    {
        foreach (var obj in ToBeSetActiveAtStart)
        {
            obj.SetActive(true);
        }
    }

    public void SetActiveEnd()
    {
        foreach (var obj in ToBeSetActivAtEnd)
        {
            obj.SetActive(true);
        }
    }

    public void SetInactiveStart()
    {
        foreach (var obj in ToSetInactiveAtStart)
        {
            obj.SetActive(false);
        }
    }


    public void SetInactiveEnd()
    {
        foreach (var obj in ToBeSetInactiveAtEnd)
        {
            obj.SetActive(false);
        }
    }
    
       
    
   
 


}

