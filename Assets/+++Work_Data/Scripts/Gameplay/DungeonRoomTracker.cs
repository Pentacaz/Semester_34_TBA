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

    private void Start()
    {
      
    }

    private void Update()
    {
        HealArea = GameObject.Find("SPAWNPOINT_ROOM_HEAL").transform;
        BossArea = GameObject.Find("SPAWNPOINT_ROOM_FINAL").transform;
        
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
