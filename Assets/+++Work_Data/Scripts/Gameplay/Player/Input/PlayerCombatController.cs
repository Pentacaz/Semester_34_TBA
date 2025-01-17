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
        StartTimer(attack);
        if(!attack)
        {
            comboCooldown = comboCooldownvalue;
            comboTimer = comboTimervalue;

        }
    }

    public void AttackHandler()
    {
        // add breakup of attack sequences and more reliable timer structure.
       currentComboCoolDown = comboCooldown;
       currentComboTimer = comboTimer;
       if ( currentComboCoolDown < 0)
       {
           if (currentComboTimer >= 0) 
           {
               comboId += 1;
           }
           if(comboId == maxCombo)
           {
               comboId = minCombo;
           }
       }
       
        AnimCallAction(comboId);
        Debug.Log($"comboId = {comboId}");
            
    }

    public void StartTimer(bool attackPerformed)
    {
        if(attackPerformed)
        {
            comboCooldown -= Time.deltaTime;
            comboTimer -= Time.deltaTime;
        }
    }

    
    public void AnimCallAction(int id)
    {
        _animator.SetTrigger("ActionTrigger");
        _animator.SetInteger("ActionId", id);
    }
    
    //#TODO MOVEMENT -> ATTACK -> ROLL -> DEFEND -> TRINKETS 
}
