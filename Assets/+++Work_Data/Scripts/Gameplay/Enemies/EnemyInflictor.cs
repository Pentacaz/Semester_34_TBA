using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInflictor : MonoBehaviour
{
   
    public int damageValue;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<PlayerReciever>().canGetDamage)
        {
            print("attack Player");
            collision.GetComponent<PlayerReciever>().GetDmg(damageValue);
        }
    }



}
