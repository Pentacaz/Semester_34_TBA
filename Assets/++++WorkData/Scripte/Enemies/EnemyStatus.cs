using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
  
  
    public int enemyLevel;
    public float enemyMaxHp;
    public int enemyDmg;
    public int enemyDefense;
    public string enemyName;
    public int enemySpeed;
    public int enemyBaseDmg;
    public int enemyBaseDefense;
    public float enemyAttackCooldown;
    
    
    public void SetUpEnemy(Unit unit)
    {
   
        enemyName = unit.unitName;
        enemyLevel = unit.unitLevel ;
        enemyBaseDefense = unit.defense;
        enemyBaseDmg = unit.damage;
        enemyMaxHp = unit.maxHp;
        enemySpeed = unit.speed;
        enemyAttackCooldown = unit.attackCooldown;
    
        
        enemyDmg = enemyBaseDmg;
        enemyDefense = enemyBaseDefense;
    }
    
}
