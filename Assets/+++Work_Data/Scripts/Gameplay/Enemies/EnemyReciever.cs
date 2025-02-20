using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class EnemyReciever : MonoBehaviour
{
    #region takeDMG
    private VisualEffect _vfx;
    public float currentHp;
    
    public bool hasShields;
    public float currentShields;
    #region Damage Indicator

    public CinemachineFreeLook vcam;
    public bool tookDamage;
    public bool canGetDamage = true;
    public float invincibilityTimer = 1.5f;
    private float _invincibilityTimerValue;
    public float knockback;
    private SpawnEnemies _spawnEnemies;
    public Image damageIndicator;
    public Image shieldIndicator;
    public TextMeshProUGUI damageText;
    #endregion
  
    #endregion

    
    #region References

    private CamBehavior _camBehavior;
    private EnemyStatus _enemyStatus;
    private Rigidbody _rigidbody;
    private Animator _animator;
    public GameObject loot;
    
    #endregion
    private void Awake()
    {
        _enemyStatus = GetComponent<EnemyStatus>();
        _camBehavior = GetComponent<CamBehavior>();
        _vfx = GetComponentInChildren<VisualEffect>();
        _rigidbody = GetComponent<Rigidbody>();
        _spawnEnemies = FindObjectOfType<SpawnEnemies>();

    }
    
    private void Start()
    {Debug.Log("enemyreciever");
        currentHp = _enemyStatus.enemyMaxHp;
        _invincibilityTimerValue = invincibilityTimer;
        DamageIndication(damageIndicator,_enemyStatus.enemyMaxHp,currentHp,0);;
        SetShields();
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
            if (hasShields)
            {
                currentShields -= dmg / 2f;
            }
            else
            {
                currentHp -= dmg; 
            }
         


            if (currentHp > _enemyStatus.enemyMaxHp)
            {
                currentHp = _enemyStatus.enemyMaxHp;
            }

            DamageIndication(damageIndicator,_enemyStatus.enemyMaxHp,currentHp,dmg);
        }
        
    }
    
    public void DamageIndication(Image damageind, float maxHpval , float currentHpval, int damagevalue)
    {
       
        if (hasShields)
        {
            shieldIndicator.fillAmount = currentShields / _enemyStatus.enemyDefense;
        }
     
        if (tookDamage)
        { 
            damageText.enabled = true;
            _vfx.Play();
            Pushback();
          _camBehavior.CamShake();
         
          damageText.SetText($"{damagevalue}");
            Debug.Log("TOOK DAMAGE ENEMY");
        }

        if (currentHp <= 0)
        {
              Destroy(this.gameObject);
             // this.gameObject.SetActive(false);
              _spawnEnemies.RemoveDefeatedEnemy(this.gameObject);
              loot.transform.position = this.gameObject.transform.position;
              loot.SetActive(true);
              //play vfx
            
            //deathIndicator.SetActive(true);
            Debug.Log("DEATH ENEMY");
        }

        if (currentShields <= 0)
        {
            hasShields = false;
            damageind.fillAmount = currentHpval / maxHpval;
            shieldIndicator.enabled = false;
            //play vfx
        }
    }

  
    
    public void Invincibility(bool damage)
    {
        if (damage)
        {
            canGetDamage = false;
            //_enemyAuraBehaviour.StopPatrol();
            invincibilityTimer -= Time.deltaTime;
        }

        if (invincibilityTimer <= 0)
        {   tookDamage = false;
            canGetDamage = true;
            //_enemyAuraBehaviour.ResumePatrol();
            invincibilityTimer = _invincibilityTimerValue;
        }
    }

    public void Pushback()
    {
        print("PUSHED!!!!");
        _rigidbody.AddForce(-this.gameObject.transform.position * knockback, ForceMode.Impulse);
    }

    public void SetShields()
    {
        float shieldSpawnChance = 0.2f;
        if (Random.value < shieldSpawnChance)
        {
            hasShields = true;
            shieldIndicator.enabled = true;
            currentShields = _enemyStatus.enemyDefense;
            
        }
    }
}
