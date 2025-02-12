using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerInflictor : MonoBehaviour
{
 
    [SerializeField] private int damageValues;


    private void Awake()
    {
       
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Attack enemy");
            collision.GetComponent<EnemyReciever>().GetDmg(damageValues);
           
        }
    }

    
}
