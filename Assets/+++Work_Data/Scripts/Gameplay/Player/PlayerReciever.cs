using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReciever : MonoBehaviour
{
   // PlayerController pController;
    //PlayerInfo playerInfo;
    public bool tookDamage;
    public float forcepower = 5f;

    private void Start()
    {
        //playerInfo = GetComponent<PlayerInfo>();
        //pController = GetComponent<PlayerController>();
    }
  
    public void GetDmg(float dmg)
    {
        //playerInfo.Damage(dmg);
        tookDamage = true;
        //pController.AnimCallAction(20);

    }

  
}
