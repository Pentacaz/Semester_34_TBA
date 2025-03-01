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
    public Image _healthBar;
    public TextMeshProUGUI healthAmount;
    #endregion
    
    #region DeathScreen

    public GameObject deathScreen;
    #endregion
   

    public void RefreshHealthbar(float maxHp, float currentHp)
    {
        _healthBar.fillAmount = currentHp / maxHp;
    }

    
    

}
