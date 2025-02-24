using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReciever : MonoBehaviour
{   public GameObject spawnPoint;
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
    private Rigidbody _rigidbody;
    public float knockback;
    public bool _setSpawn;
       
    private void Start()
    {
        //playerInfo = GetComponent<PlayerInfo>();
        _uiManager = Component.FindObjectOfType<UiManager>();
        pController = GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody>();
        _invincibilityTimerValue = invincibilityTimer;
        _uiManager.RefreshHealthbar(maxHp,currentHp);
        _setSpawn = true;
    }


    private void Update()
    {
        Invincibility();
    }

    public void Spawn(bool spwn)
    {
        if (spwn)
        {
            this.gameObject.transform.position = spawnPoint.transform.position;
            
        }

        if (  this.gameObject.transform.position == spawnPoint.transform.position)
        {
            _setSpawn = false;
            spawnPoint.SetActive(false);
        }
      
    }

    public void GetDmg(int dmg)
    {
        if (canGetDamage)
        {
            tookDamage = true;
            currentHp -= dmg;
            Pushback();

            if (currentHp > maxHp)
            {
                currentHp = maxHp;
            }
            _uiManager.RefreshHealthbar(maxHp, currentHp);
            if (currentHp < 1)
            {
                //pController.AnimationsAction(10);
            }
        }
    }

    public void GetHeal()
    {
        
        if (currentHp >= maxHp)
        {
            Debug.Log("Player is already at max health. No heal used.");
            return;
        }
        
        healCount++;


        if (healCount >= maxHeal)
        {
            Debug.Log("No heals remaining!");
            return;
        }
        else
        {
            int amount = (int)(maxHp * 0.25f);
            currentHp += amount;

            // Ensure health doesn't exceed max health
            if (currentHp > maxHp)
            {
                currentHp = maxHp;
            }

            _uiManager.RefreshHealthbar(maxHp, currentHp);
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

    public void Pushback()
    {
        print("PUSHED!!!!");
        _rigidbody.AddForce(-this.gameObject.transform.position * knockback, ForceMode.Impulse);
    }
    
}
