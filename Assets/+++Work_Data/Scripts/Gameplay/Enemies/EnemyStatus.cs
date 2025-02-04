using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
   
    public Animator anim;
    public SkinnedMeshRenderer enemyMesh;
    
    public int enemyLevel;
    public int enemyMaxHp;
    public int enemyDmg;
    public int enemyDefense;
    public string enemyName;
    public int enemySpeed;
    public int enemyBaseDmg;
    public int enemyBaseDefense;
    public float enemyAttackCooldown;
    
    enum EnemyState
    {
        PATROL,
        CHASE,
        ATTACK
    }
    
    public void SetUpEnemy(Unit unit)
    {
        anim.runtimeAnimatorController = unit.animController;
        enemyMesh.sharedMesh = unit.unitMesh;
        //enemyMesh.rootBone = unit.unitRig.GetComponent<>();
        enemyName = unit.unitName;
        enemyLevel = unit.unitLevel ;
        enemyBaseDefense = unit.defense;
        enemyBaseDmg = unit.damage;
        enemyMaxHp =unit.maxHp;
        enemySpeed = unit.speed;
        enemyAttackCooldown = unit.attackCooldown;
    
        
        enemyDmg = enemyBaseDmg;
        enemyDefense = enemyBaseDefense;
    }
    
}
