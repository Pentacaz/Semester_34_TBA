using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerInflictor : MonoBehaviour
{
 
    [SerializeField] private int damageValues;
    private VisualEffect _vfx;

    private void Awake()
    {
        _vfx = GetComponentInChildren<VisualEffect>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Attack enemy");
            collision.GetComponent<EnemyReciever>().GetDmg(damageValues);
            _vfx.Play();
        }
    }

    
}
