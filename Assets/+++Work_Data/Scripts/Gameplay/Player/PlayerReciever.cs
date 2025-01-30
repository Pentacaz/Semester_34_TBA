using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReciever : MonoBehaviour
{
    PlayerController pController;
    private UiManager _uiManager;
    public int healCount;
    public int maxHeal;
    public bool tookDamage = false;
    public int maxHp = 25;
    public int currentHp;
    public bool canGetDamage = true;
    public float invincibilityTimer = 1.5f;
    public float _invincibilityTimerValue;

    private void Start()
    {
        //playerInfo = GetComponent<PlayerInfo>();
        _uiManager = Component.FindObjectOfType<UiManager>();
        pController = GetComponent<PlayerController>();
        _invincibilityTimerValue = invincibilityTimer;
        _uiManager.RefreshHealthbar(maxHp,currentHp);
    }


    private void Update()
    {
        Invincibility();
    }

    public void GetDmg(int dmg)
    {
        if (canGetDamage)
        {
            tookDamage = true;
            currentHp -= dmg;
            //pController.AnimationsAction(5);

            if (currentHp > maxHp)
            {
                currentHp = maxHp;
            }
        
            _uiManager.RefreshHealthbar(maxHp,currentHp);

            if (currentHp < 1)
            {
                //pController.AnimationsAction(10);
            }
        }
       
    
    }

    public void GetHeal(int amount)
    {
        healCount++;
        
        
        if (healCount >= maxHeal)
        {
            Debug.Log("No heals remaining!");
            return;
        }
        else
        {
            currentHp += amount;

            // Ensure health doesn't exceed max health
            if (currentHp > maxHp)
            {
                currentHp = maxHp;
            }
            _uiManager.RefreshHealthbar(maxHp,currentHp);
            Debug.Log($"Player healed by {amount}. Current health: {currentHp}/{maxHp}");
        }
        
    
            
    }
    
    public void Invincibility()
    {
        if (tookDamage)
        {
            canGetDamage = false;
            
            invincibilityTimer -= Time.deltaTime;
        }

        if (invincibilityTimer <= 0)
        {   tookDamage = false;
            canGetDamage = true;
            invincibilityTimer = _invincibilityTimerValue;
        }
    }
}
