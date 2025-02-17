using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInflictor : MonoBehaviour
{
    public bool isAuraEnemy;
    public float damagePerSecond = 1f;
    public float interval = 1f;


  
    private EnemyStatus _enemyStatus;
    private float _damageTimer;
    private bool _isPlayerInTrigger;
    private PlayerReciever _playerReceiver;

    private void Awake()
    {
        _enemyStatus = GetComponentInParent<EnemyStatus>();
    }

    private void Start()
    {
        damagePerSecond = _enemyStatus.enemyDmg;
        _damageTimer = interval;
    }

    private void Update()
    {
        if (_isPlayerInTrigger && isAuraEnemy && gameObject.activeSelf)
        {
            DamageOverTime();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerReciever>().canGetDamage)
        {
            _isPlayerInTrigger = true;
            _playerReceiver = other.GetComponent<PlayerReciever>();
          
            if (!isAuraEnemy)
            {
                Debug.Log("Attack Player: " + other.gameObject.name);
                _playerReceiver.GetDmg(_enemyStatus.enemyDmg);
            }
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInTrigger = false;
        }
    }

    private void DamageOverTime()
    {
        _damageTimer -= Time.deltaTime;

        if (_damageTimer <= 0)
        {
            int dotDamage = (int)(damagePerSecond * interval);
            _playerReceiver.GetDmg(dotDamage);
            _damageTimer = interval;
        }
    }
    
    

}



       
       

      
    
    
