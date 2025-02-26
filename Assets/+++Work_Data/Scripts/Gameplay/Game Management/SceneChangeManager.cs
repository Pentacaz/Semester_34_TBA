using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    //this script is screaming for the sweet release of death
    public GameObject playerContainer; 
    private Transform spawnPoint;
    private bool spawned = true;
    private SaveGameData _saveGameData;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _saveGameData = FindObjectOfType<SaveGameData>();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        spawnPoint = GameObject.FindWithTag("SpawnPoint")?.transform;
        playerContainer.transform.position = spawnPoint.position;
        spawned = true;
        if (_saveGameData != null)
        {
           _saveGameData.LoadStates("gameObjectsState.json");
        }
        
    }

    private void Update()
    {
        if (spawned)
        {
            StartCoroutine(DisableSpawn());
        }
    }

    IEnumerator DisableSpawn()
    {
        yield return new WaitForSeconds(0.5f);
        spawned = false;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
    


