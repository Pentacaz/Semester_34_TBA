using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Drawing;
using TMPro;
public class UiManager : MonoBehaviour
{
    #region Healthbar
    private Image _healthBar;
    #endregion


    //EnemyReciever enemyreciever;
    //PlayerReciever playerReciever;

    public void RefreshHealthbar(float maxHp, float currentHp)
    {
        _healthBar.fillAmount = currentHp / maxHp;
    }


}
