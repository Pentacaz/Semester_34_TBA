using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float maxHp = 10;
    [SerializeField] private float currentHp;
    
    private UiManager _uiManager;
   

  

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Damage(float dmg)
    {
        currentHp -= dmg;
        
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        
        //_uiManager.RefreshHealthbar(maxHp, currentHp);
    }
    
    public void Heal(float hp)
    {
        currentHp += hp;

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        
        //_uiManager.RefreshHealthbar(maxHp, currentHp);
    }

    
}
