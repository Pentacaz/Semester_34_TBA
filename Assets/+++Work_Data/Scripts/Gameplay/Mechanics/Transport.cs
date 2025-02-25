using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Transport : MonoBehaviour
{
 public List<Transform> spawnPoints;
 private DungeonRoomTracker _dungeonRoomTracker;

 private void Awake()
 {
     _dungeonRoomTracker = FindObjectOfType<DungeonRoomTracker>();
 }

 private void Update()
 {
     if (_dungeonRoomTracker.canAccessHealing)
     {
                 spawnPoints.Add(_dungeonRoomTracker.HealArea);
                 
     }

     if (_dungeonRoomTracker.canAccessBoss)
     {
         spawnPoints.Clear();
         spawnPoints.Add(_dungeonRoomTracker.BossArea);
         _dungeonRoomTracker.canAccessBoss = false;
     }
 }

 private void OnTriggerEnter(Collider other)
 {
     if (other.CompareTag("Player"))
          {
             
              int index = Random.Range(0, spawnPoints.Count);
              other.gameObject.transform.position = spawnPoints[index].position;
          }
 }

 
 
}
