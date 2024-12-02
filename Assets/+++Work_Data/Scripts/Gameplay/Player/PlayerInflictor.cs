using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerInflictor : MonoBehaviour
{
    [SerializeField] private int dealDmgId;
    [SerializeField] private float[] damageValues;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Attack enemy");
            //collision.GetComponent<EnemyReceiver>().GetDmg(damageValues[dealDmgId]);
        }
    }
}
