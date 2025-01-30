using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class EnemyReciever : MonoBehaviour
{
    #region takeDMG

    public EnemyBehaviour enemyBehaviour;
    public int currentHp;
    public int maxHp = 15;
    
    public bool tookDamage;
    public Image damageIndicator;
    private Rigidbody _rigidbody;
    public float pushForce;
    
    public bool canGetDamage = true;
    public float invincibilityTimer = 1.5f;
    private float _invincibilityTimerValue;
   private Vector3 playerdirec;
   public GameObject loot;
    #endregion

    #region Behavior

    private Animator _animator;

    #endregion
    
    
   
    ///public GameObject deathIndicator;
   public enum EnemyBehaviourOnDeath
    {
        SELFDESTRUCT
        
    }

    public EnemyBehaviourOnDeath enemyBehaviourOnDeath;

    private void Awake()
    {
     
    }
    
    private void Start()
    {
       
         playerdirec =FindObjectOfType<PlayerBaseController>()._playerDirection;
         _rigidbody = GetComponent<Rigidbody>();
        _invincibilityTimerValue = invincibilityTimer;
        DamageIndication(damageIndicator,maxHp,currentHp);;
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


            if (currentHp > maxHp)
            {
                currentHp = maxHp;
            }

            DamageIndication(damageIndicator,maxHp,currentHp);
        }
       
        
    }
        
    

    public void DamageIndication(Image damageind,float maxHpval, float currentHpval)
    {
        
      
        damageind.fillAmount = currentHpval / maxHpval;
        
        if (tookDamage)
        {
           
            PushEnemy(playerdirec);

            
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

    public void PushEnemy(Vector3 damageSourcePosition)
    {
        Vector3 pushDirection = (transform.position - damageSourcePosition).normalized;

        
        _rigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
    }
    public void Invincibility(bool damage)
    {
        if (damage)
        {
            canGetDamage = false;
            enemyBehaviour.StopPatrol();
            invincibilityTimer -= Time.deltaTime;
        }

        if (invincibilityTimer <= 0)
        {   tookDamage = false;
            canGetDamage = true;
            enemyBehaviour.ResumePatrol();
            invincibilityTimer = _invincibilityTimerValue;
        }
    }

   
}


