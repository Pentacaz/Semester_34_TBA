using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInteract : MonoBehaviour
{
    private PlayerReciever _playerReciever;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _playerReciever = FindObjectOfType<PlayerReciever>();
    }


    public void Heal()
    {
        _playerReciever.GetHeal();
    }
}
