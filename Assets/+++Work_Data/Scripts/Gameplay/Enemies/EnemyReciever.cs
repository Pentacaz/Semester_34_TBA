using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class EnemyReciever : MonoBehaviour
{
    #region takeDMG
    
    public int currentHp;
    
    #region Damage Indicator 
    
    public bool tookDamage;
    public bool canGetDamage = true;
    public float pushForce;
    public float invincibilityTimer = 1.5f;
    private float _invincibilityTimerValue;
    
    public Image damageIndicator;

    #endregion
  
  

    #endregion

    #region References
    
    private EnemyBehaviour _enemyBehaviour;
    private EnemyStatus _enemyStatus;
    
    private Rigidbody _rigidbody;
    
    private Animator _animator;
    
  
    public GameObject loot;
    
    #endregion
    private void Awake()
    {
        _enemyStatus = GetComponent<EnemyStatus>();
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Start()
    {
        _invincibilityTimerValue = invincibilityTimer;
        DamageIndication(damageIndicator,_enemyStatus.enemyMaxHp,currentHp);;
    }
    private void Update()
    {
      
        Invincibility(tookDamage);
        
    }
    
    public void GetDmg(int dmg)
    {
        if (canGetDamage)
        {
            tookDamage = true;
            currentHp -= dmg;


            if (currentHp > _enemyStatus.enemyMaxHp)
            {
                currentHp = _enemyStatus.enemyMaxHp;
            }

            DamageIndication(damageIndicator,_enemyStatus.enemyMaxHp,currentHp);
        }
       
        
    }
    
    public void DamageIndication(Image damageind,float maxHpval, float currentHpval)
    {
        
      
        damageind.fillAmount = currentHpval / maxHpval;
        
        if (tookDamage)
        {
            PushEnemy();
            Debug.Log("TOOK DAMAGE ENEMY");
        }

        if (currentHp <= 0)
        {
           
              this.gameObject.SetActive(false);
              loot.SetActive(true);
              
            
            //deathIndicator.SetActive(true);
            Debug.Log("DEATH ENEMY");
        }
        // if i had used a regular enemy id call the animator and play the death animation. 
        //there is probs a way to solve this better,
    }

    public void PushEnemy()
    {
        _rigidbody.AddForce(_rigidbody.velocity * -1, ForceMode.Impulse);
    }
    public void Invincibility(bool damage)
    {
        if (damage)
        {
            canGetDamage = false;
            _enemyBehaviour.StopPatrol();
            invincibilityTimer -= Time.deltaTime;
        }

        if (invincibilityTimer <= 0)
        {   tookDamage = false;
            canGetDamage = true;
            _enemyBehaviour.ResumePatrol();
            invincibilityTimer = _invincibilityTimerValue;
        }
    }

   
}


