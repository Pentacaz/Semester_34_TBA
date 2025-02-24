using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class PlayerInflictor : MonoBehaviour
{

    [SerializeField] private int damageValues;
    [SerializeField] private int damageBaseValues;
    [SerializeField] private int finalDamage;

    [SerializeField] float critChance = 0.05f;
    [SerializeField] float critMultiplier = 2f;
    public bool isCritDamage;

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

            isCritDamage = Random.value < critChance;
            finalDamage = damageBaseValues;

            switch (_playerCombatController.comboId)
            {
                case 1:
                    finalDamage = damageBaseValues;
                    break;
                case 2:
                    finalDamage = (int)(damageBaseValues * 1.5f);
                    break;
                case 3:
                    finalDamage = (int)(damageBaseValues * 2f);
                    break;
            }

            if (isCritDamage)
            {
                finalDamage = (int)(finalDamage * critMultiplier);
                Debug.Log("CRITICAL HIT (I CANT STOP WINNING!)");
            }

            collision.GetComponent<EnemyReciever>().GetDmg(finalDamage,isCritDamage);
            damageValues = damageBaseValues;
        }
    }


}
