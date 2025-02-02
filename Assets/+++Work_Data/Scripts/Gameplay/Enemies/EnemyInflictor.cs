using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInflictor : MonoBehaviour
{
    public bool canAttack = true;
    public bool attacked = false;
    public float attackCooldown;
    private float _attackCooldownvalue;
    public int damageValue;

    private void Start()
    {
        _attackCooldownvalue = attackCooldown;
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
            collision.GetComponent<PlayerReciever>().GetDmg(damageValue);
        }
    }

    public void AttackCooldown()
    {
        if (attacked)
        {
            canAttack = false;
            
            attackCooldown -= Time.deltaTime;
        }

        if (attackCooldown <= 0)
        {   attacked = false;
            canAttack = true;
            attackCooldown = _attackCooldownvalue;
        }
    }

}
