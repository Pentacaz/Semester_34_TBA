using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Transport : MonoBehaviour
{
 public List<Transform> spawnPoints;
 

 private void OnTriggerEnter(Collider other)
 {
     if (other.CompareTag("Player"))
          {
              
              int index = Random.Range(0, spawnPoints.Count);
              other.gameObject.transform.position = spawnPoints[index].position;
          }
 }

}
