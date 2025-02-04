using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NavMeshPAtrolStartAtRandom : MonoBehaviour
{
  [SerializeField] private GameObject[] customers;

  [SerializeField] private NavMeshPatrol navMeshPatrol;

  private void Awake()
  {
    navMeshPatrol = GetComponent<NavMeshPatrol>();
  }

  public void RandomOrder()
  {
    //Random.Range(customers.Length);
  }
  
  
  
}
