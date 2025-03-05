using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Transport : MonoBehaviour
{
    public List<Transform> spawnPoints;
    private DungeonRoomTracker _dungeonRoomTracker;
    private SpawnEnemies[] _spawnEnemies;
    private bool _canTravel = true;

    private void Awake()
    {
        _dungeonRoomTracker = FindObjectOfType<DungeonRoomTracker>();
        _spawnEnemies = FindObjectsOfType<SpawnEnemies>();

    }

    private void Update()
    {
        if ( _dungeonRoomTracker != null && _dungeonRoomTracker.canAccessHealing )
        {
            AddObjectToList(_dungeonRoomTracker.HealArea);

        }

        if ( _dungeonRoomTracker != null &&_dungeonRoomTracker.canAccessBoss )
        {
            spawnPoints.Clear();
            AddObjectToList(_dungeonRoomTracker.BossArea);

        }

     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _canTravel)
        {

            int index = Random.Range(0, spawnPoints.Count);
            other.gameObject.transform.position = spawnPoints[index].position;
            spawnPoints.RemoveAt(index);
            Debug.Log($"{index}");
            foreach (var obj in _spawnEnemies)
            {
                obj.triggerCollider.enabled = true;
            }
        }
    }


    void AddObjectToList(Transform obj)
    {

        if (!spawnPoints.Contains(obj))
        {

            spawnPoints.Add(obj);
            Debug.Log(obj.name + " added to the list.");
        }
        else
        {

            Debug.Log(obj.name + " is already in the list.");
        }
    }


}
 

