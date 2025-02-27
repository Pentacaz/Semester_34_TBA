using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    //this script is screaming for the sweet release of death
    public GameObject playerContainer;
    public GameObject playerBakeryContainer;
    public List<GameObject> dungeonObjects;
    public List<GameObject> bakeryObjects;
    private Transform spawnPoint;
    private bool spawned = true;
    private SaveGameData _saveGameData;
    private GameObject _dungeonIndicator;
    private GameObject _bakeryIndicator;
    private void Awake()
    {
     
        _saveGameData = FindObjectOfType<SaveGameData>();
       
        
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
     
        _dungeonIndicator = GameObject.Find("DungeonIndicator");
        _bakeryIndicator = GameObject.Find("BakeryIndicator");
        spawnPoint = GameObject.FindWithTag("SpawnPoint")?.transform;
        playerContainer.transform.position = spawnPoint.position;
        spawned = true;
        if (_saveGameData != null)
        {
           _saveGameData.LoadStates("gameObjectsState.json");
        }

       
    }

    private void Update()
    {   ControlActivePlayer();
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


    public void ControlActivePlayer()
    {
        if (_dungeonIndicator == null)
        {
            playerBakeryContainer.SetActive(true);
            playerContainer.SetActive(false);
            foreach (GameObject obj in dungeonObjects)
            {
                obj.SetActive(false);
            }
            
        }
        else
        {
            playerBakeryContainer.SetActive(false);
            playerContainer.SetActive(true);

            foreach (GameObject obj in dungeonObjects)
            {
                obj.SetActive(true);
            }
            
        }

        if (_bakeryIndicator == null)
        {
            foreach (GameObject obj in bakeryObjects)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject obj in bakeryObjects)
            {
                obj.SetActive(true);
            }
        }
    }


}
    


