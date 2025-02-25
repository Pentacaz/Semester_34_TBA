using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomTracker : MonoBehaviour
{
    public int clearedRooms;
    public int maxRooms;
    public bool canAccessHealing;
    public bool canAccessBoss;

    public Transform HealArea;
    public Transform BossArea;
    private PlayerReciever _playerReciever;
    private void Awake()
    {
        _playerReciever = GetComponent<PlayerReciever>();
    }

    private void Update()
    {
        if (_playerReciever.currentHp != _playerReciever.maxHp)
        {
            canAccessHealing = true;
        }

        if (clearedRooms == maxRooms)
        {
            canAccessBoss = true;
        }
    }

    public void AddClearedRoom()
    {
        clearedRooms++;
    }
}
