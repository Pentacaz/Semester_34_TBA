using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InstantiateEnemy : MonoBehaviour
{
    public List<Unit> units;
    private EnemyStatus _enemyStatus;

    private void Awake()
    {
        _enemyStatus = GetComponent<EnemyStatus>();
    }

    void Start()
    {
        SetUpEnemyStatus();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpEnemyStatus()
    {
       int index = Random.Range(0, units.Count);
       _enemyStatus.SetUpEnemy(units[index]);
    }
}
