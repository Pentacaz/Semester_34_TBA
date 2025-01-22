using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public int trapId;
    public int damagevalue;
    private PlayerReciever _playerReciever;
    void Start()
    {
        _playerReciever = FindObjectOfType<PlayerReciever>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TrapType(int id)
    {
        switch (id)
        {
            case 1 :
                SimpleDamage(damagevalue);
                break;
            case 2:
                PoisonDamage(damagevalue);
                break;
            case 3:
                Slow(damagevalue);
                break;
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<PlayerReciever>().canGetDamage)
        {
            TrapType(trapId);
        }
    }

//Add cooldown
    public void SimpleDamage(int dmg)
    {
        _playerReciever.GetDmg(dmg);
    }

    public void PoisonDamage(int dmg)
    {
        //overtime damage
    }

    public void Slow(int slow)
    {
        
    }
}
