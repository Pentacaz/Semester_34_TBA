using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInflictor : MonoBehaviour
{
  
    private EnemyState _enemyState;
  
    private EnemyStatus _enemyStatus;

    private void Awake()
    {
        _enemyStatus = GetComponentInParent<EnemyStatus>();
    }

    private void Start()
    {
       
       
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") &&  FindObjectOfType<PlayerReciever>().canGetDamage)
        {
            print("attack Player" + collision.gameObject.name);
            collision.GetComponent<PlayerReciever>().GetDmg(_enemyStatus.enemyDmg);
        }
    }

}
