using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveHitTrigger : MonoBehaviour
{
   public GameObject _weaponCollider;

    private void Awake()
    {
        
    }

    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetActiveWp()
    {
        _weaponCollider.SetActive(true);
    }
    public void SetInactiveWp()
    {
        _weaponCollider.SetActive(false);
    }
}
