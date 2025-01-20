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
    public int maxCombo;
    public int minCombo = 1;

  public float currentComboCoolDown;
 public float currentComboTimer;
    
    private void Awake()
    {

        comboCooldownvalue = comboCooldown;
        comboTimervalue = comboTimer;
        comboId = minCombo;
        _playerBaseController = GetComponent<PlayerBaseController>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
       
     
        if(!attack)
        {
            ResetTimers();
            currentComboCoolDown = comboCooldown;
            currentComboTimer = comboTimer;
        }
        else
        {
            StartTimer();
        }
    }

    public void AttackHandler()
    {
        
       if ( currentComboCoolDown <= 0 && currentComboTimer >= 0)
       {
           comboId += 1;
           
           if(comboId >= maxCombo)
           {
               comboId = minCombo; 
           }
       }

       currentComboCoolDown = comboCooldown;
       currentComboTimer = comboTimer;
        AnimCallAction(comboId);
        Debug.Log($"comboId = {comboId}");
            
    }

    public void StartTimer()
    {
        comboCooldown -= Time.deltaTime;
        comboTimer -= Time.deltaTime;

        if (comboTimer <= 0)
        {
            comboId = minCombo; 
        }
        
    }

    public void ResetTimers()
    {
        
      
        comboTimer = comboTimervalue; 
        comboCooldown = comboCooldownvalue;
        
        
        
    }
    
    public void AnimCallAction(int id)
    {
        _animator.SetTrigger("ActionTrigger");
        _animator.SetInteger("ActionId", id);
    }
    
    //#TODO MOVEMENT -> ATTACK -> ROLL -> DEFEND -> TRINKETS 
}
