using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public Projectiles projectileType;
    public GameObject projectilePrefab;
    public Transform direction;

    // Update is called once per frame


    private void Start()
    {
        InvokeRepeating("Fire",1f,1f);
    }


    void Fire()
    {
        var projectile = Instantiate(projectilePrefab, direction.position, direction.rotation);
        projectile.GetComponent<ProjectileBehavior>().projectile = projectileType;
    }
}
