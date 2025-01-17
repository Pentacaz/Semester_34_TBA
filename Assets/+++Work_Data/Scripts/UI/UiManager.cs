using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Drawing;
using TMPro;
using UnityEngine.InputSystem.XR.Haptics;
using Color = UnityEngine.Color;

public class UiManager : MonoBehaviour
{
    #region Healthbar

    public float currentHp;
    public float maxHp;
    public Image healthBar;
    #endregion

    private void Update()
    {
       RefreshHealthBar();
    }

    //EnemyReciever enemyreciever;
    //PlayerReciever playerReciever;
    // public void RefreshHealthbar(float maxHp, float currentHp)
    public void RefreshHealthBar()
    {
        healthBar.fillAmount = currentHp / maxHp;
        if (healthBar.fillAmount < 0.5f)
        {//do properly
            healthBar.color = Color.Lerp(Color.green, Color.red,0.5f);
        }
    }


}
