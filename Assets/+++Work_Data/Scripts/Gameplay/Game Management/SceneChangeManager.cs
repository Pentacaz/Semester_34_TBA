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
     private void Start()
    {
        Debug.Log("SceneLoader started.");
        
   
       

        SceneManager.sceneLoaded += OnSceneLoaded;
        InitializeScene();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded called for scene: " + scene.name);


        _dungeonIndicator = GameObject.Find("DungeonIndicator");
        _bakeryIndicator = GameObject.Find("BakeryIndicator");
        spawnPoint = GameObject.FindWithTag("SpawnPoint")?.transform;

        playerContainer.transform.position = spawnPoint.position;
        playerBakeryContainer.transform.position = spawnPoint.position;
        
        //SaveGameData.Instance.LoadStates("gameObjectsState.json");

        ControlActivePlayer();
    }
    

    private void InitializeScene()
    {
        ControlActivePlayer();
    }

    private void ControlActivePlayer()
    {
     
        if (_dungeonIndicator == null)
        {
          
            playerBakeryContainer.SetActive(true);
            playerContainer.SetActive(false);
            SetObjectsActive(dungeonObjects, false);
        }
        else
        {
           
            playerBakeryContainer.SetActive(false);
            playerContainer.SetActive(true);
            SetObjectsActive(dungeonObjects, true);
        }

        if (_bakeryIndicator == null)
        {
          
            SetObjectsActive(bakeryObjects, false);
        }
        else
        {
          
            SetObjectsActive(bakeryObjects, true);
        }
    }

    private void SetObjectsActive(List<GameObject> objects, bool isActive)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(isActive);
            }
        }
    }

    private void OnDestroy()
    {
        Debug.Log("SceneLoader destroyed.");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}


    


