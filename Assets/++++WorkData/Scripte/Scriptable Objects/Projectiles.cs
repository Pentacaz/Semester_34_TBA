using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Projectile", menuName = "ScriptableObjects/Projectile", order = 3)]

public class Projectiles : ScriptableObject
{
    [Header("Stats")] 
    public Mesh projectileMesh;
    
    [Range(50f,500f)]
    public float velocity; 
    
    [Range(0.001f, 0.050f)]
    public float mass;
    
    public float KineticEnergyDamage
    {
        get => (float)((0.5f * mass) * Math.Pow(velocity, 2f));
    }

}
