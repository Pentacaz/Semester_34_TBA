using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
   
    public Animator anim;
    
    public int enemyLevel;
    public int enemyMaxHp;
    public int enemyDmg;
    public int enemyDefense;
    public string enemyName;
    public int enemySpeed;
    public int enemyBaseDmg;
    public int enemyBaseDefense;
    
    enum EnemyState
    {
        PATROL,
        CHASE,
        ATTACK
    }
    
    public void SetUpEnemy(Unit unit)
    {
        anim.runtimeAnimatorController = unit.animController;
        
        enemyName = unit.unitName;
        enemyLevel = unit.unitLevel ;
        enemyBaseDefense = unit.defense;
        enemyBaseDmg = unit.damage;
        enemyMaxHp =unit.maxHp;
        enemySpeed = unit.speed;
    
        
        enemyDmg = enemyBaseDmg;
        enemyDefense = enemyBaseDefense;
    }
    
}
