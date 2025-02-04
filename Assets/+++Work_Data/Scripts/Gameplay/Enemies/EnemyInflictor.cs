using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInflictor : MonoBehaviour
{
    public bool canAttack = true;
    public bool attacked = false;
    private float _attackCooldownvalue;
    private EnemyStatus _enemyStatus;

    private void Awake()
    {
        _enemyStatus = GetComponent<EnemyStatus>();
    }

    private void Start()
    {
       
        _attackCooldownvalue = _enemyStatus.enemyAttackCooldown;
    }

    private void Update()
    {
        AttackCooldown();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") &&  FindObjectOfType<PlayerReciever>().canGetDamage)
        {
            print("attack Player");
            collision.GetComponent<PlayerReciever>().GetDmg(_enemyStatus.enemyDmg);
        }
    }

    public void AttackCooldown()
    {
        if (attacked)
        {
            canAttack = false;
            
            _enemyStatus.enemyAttackCooldown -= Time.deltaTime;
        }

        if (_enemyStatus.enemyAttackCooldown <= 0)
        {   attacked = false;
            canAttack = true;
            _enemyStatus.enemyAttackCooldown = _attackCooldownvalue;
        }
    }

}
