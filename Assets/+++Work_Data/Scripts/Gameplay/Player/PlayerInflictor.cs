using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerInflictor : MonoBehaviour
{
 
    [SerializeField] private int damageValues;
    [SerializeField] private int damageBaseValues;
    private PlayerCombatController _playerCombatController;

    private void Awake()
    {
        _playerCombatController = GetComponentInParent<PlayerCombatController>();
    }

    private void Start()
    {
        damageBaseValues = damageValues;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Attack enemy");

            switch (_playerCombatController.comboId)
            {
                case 1 :
                    collision.GetComponent<EnemyReciever>().GetDmg(damageValues);
                    break;
                case 2 :
                    damageValues = (int)(damageBaseValues * 1.5f);
                    collision.GetComponent<EnemyReciever>().GetDmg(damageValues);
                    damageValues = damageBaseValues;
                    break;
                case 3:
                    damageValues = (int)(damageBaseValues * 2f);
                    collision.GetComponent<EnemyReciever>().GetDmg(damageValues);
                    damageValues = damageBaseValues;
                    break;
            }
           
           
        }
    }

    
}
