using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{

    private static readonly int Hash_ComboCooldown = Animator.StringToHash("ComboCooldown");
    private static readonly int Hash_ComboDuration = Animator.StringToHash("ComboDuration");

    private PlayerBaseController _playerBaseController;
    private Animator _animator;
    public float comboTimer;
    public float comboCooldown;
    public float comboTimervalue;
    public float comboCooldownvalue;
    public bool attack;
    public int comboId;
    public int heavyAttackId;
    public int maxCombo;
    public int minCombo = 1;
    public float heavyCooldown;
    public float heavyCooldownValue;
    public int attackId;
    public bool canAttack = true;
    public bool canHeavyAttack = true;

  public float currentComboCoolDown;
 public float currentComboTimer;
    
    private void Awake()
    {

        comboCooldownvalue = comboCooldown;
        comboTimervalue = comboTimer;
        heavyCooldownValue = heavyCooldown;
        _playerBaseController = GetComponent<PlayerBaseController>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
       CanAttack();
       ComboMemory(attackId);
       if (!attack)
       {
           ResetTimers();
       }
       else
       {
           StartTimer();
       }
        
        
    }

    public void AttackHandler()
    {
       if  (comboTimer >= 0 && currentComboTimer >= 0)
       {
           comboId += 1;
           
           if(comboId > maxCombo)
           {
               comboId = minCombo; 
           }
       }
       else
       {
           comboId = minCombo; 
       }
       
       currentComboCoolDown = comboCooldown;
       currentComboTimer = comboTimer;

        AnimCallAction(comboId);
        Debug.Log($"comboId = {comboId}");
            
    }

    public void HeavyAttackHandler()
    {
        if (heavyCooldown <= 0)
        {
            canHeavyAttack = true;
        }
    }
    
    public void StartTimer()
    {
        comboCooldown -= Time.deltaTime;
        comboTimer -= Time.deltaTime;

        if (currentComboTimer  <= 0|| comboTimer <= 0)
        {
           comboId = minCombo; 
        }
        
    }

    public void ResetTimers()
    {
        comboTimer = comboTimervalue; 
        comboCooldown = comboCooldownvalue;
        heavyCooldown = heavyCooldownValue;
    }
    
    public void CanAttack()
    {
        if (comboCooldown <= 0|| currentComboCoolDown <= 0 )
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }

    public void ComboMemory(int id)
    {
        switch (id)
        {
            case 1 :
                if (comboId > minCombo)
                {
                    currentComboTimer -= Time.deltaTime;
                }
                break;
            case 2 :
                
                heavyCooldown -= Time.deltaTime;
                
                break;
        }
    }
    public void AnimCallAction(int id)
    {
        _animator.SetTrigger("ActionTrigger");
        _animator.SetInteger("ActionId", id);
    }
    
    //#TODO MOVEMENT -> ATTACK -> ROLL -> DEFEND -> TRINKETS 
}
