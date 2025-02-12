using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Unit", order = 1)]
public class Unit : ScriptableObject
{
    //class for enemy stats/ configuration. Please do not edit unless asked.
    [Header("Essentials")] 
    public RuntimeAnimatorController animController;
    public Animator animator;
    public GameObject unitRig;
    public Mesh unitMesh;
    
   [Header("Stats")] 
    public string unitName = "Enemy Name";
    public int unitLevel = 1;
    public int defense = 0;
    public int damage = 0;
    public float maxHp = 0;
    public int speed = 0;

    [Header("Enemy Secondary Stats")] 
    public float attackCooldown;
    
    public enum EnemyType
    {
        GROUND,
        PROJECTILE,
        EXPLOSIVE
    }
}
