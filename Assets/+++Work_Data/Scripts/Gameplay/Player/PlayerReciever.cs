using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class PlayerReciever : MonoBehaviour
{   
    PlayerBaseController pController;
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
        _uiManager = Component.FindObjectOfType<UiManager>();
        pController = GetComponent<PlayerBaseController>();
        _rigidbody = GetComponent<Rigidbody>();
        _invincibilityTimerValue = invincibilityTimer;
        _uiManager.RefreshHealthbar(maxHp,currentHp);
        _setSpawn = true;
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
            //Pushback();

            if (currentHp > maxHp)
            {
                currentHp = maxHp;
            }
            _uiManager.RefreshHealthbar(maxHp, currentHp);
            if (currentHp <= 0)
            {
               StartCoroutine(nameof(PlayerDeath));
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

    
    //removed temporarily - caused issues w/ player
   // public void Pushback()
   // {
        //print("PUSHED!!!!");
        //_rigidbody.AddForce(-this.gameObject.transform.position * knockback, ForceMode.Impulse);
    //

    IEnumerator PlayerDeath()
    {
        pController.DisableInput();
        Debug.Log("DEATH PLAYER");
        if (pController._animator != null)
        {
            pController._animator.SetTrigger("ActionTrigger");
            pController._animator.SetInteger("ActionId", 1);
        }

        yield return new WaitForSeconds(1f);
        _uiManager.deathScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        pController.EnableInput();
        _uiManager.deathScreen.SetActive(false);
        SceneManager.LoadScene("Bakery");

    }
}
