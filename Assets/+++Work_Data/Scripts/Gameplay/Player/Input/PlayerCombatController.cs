using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    private PlayerBaseController _playerBaseController;

    public float comboDuration;
    public float comboCooldown;
    
    private void Awake()
    {
        _playerBaseController = GetComponent<PlayerBaseController>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //#TODO MOVEMENT -> ATTACK -> ROLL -> DEFEND -> TRINKETS 
}
