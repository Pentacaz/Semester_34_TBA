using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveGameData : MonoBehaviour
{

    /// <summary>
    /// call SaveStates("gameObjectsState.json"); to save and  LoadStates("gameObjectsState.json"); to load. (this is simply thw string I put in the inspector)
    /// </summary>
    public static SaveGameData Instance { get; private set; }

    [System.Serializable]
    private class GameObjectState
    {
        public string name; 
        public bool isActive;

    }

    [System.Serializable]
    private class GameObjectStateCollection
    {
        public List<GameObjectState> gameObjectStates = new List<GameObjectState>();
       
    }

  ;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    /// <summary>
    /// Finds ALL gameobjects in scene , filter them by Tag. GameobjectState contains all the attributes that are meaant to be saved.
    /// creates an instance (?) of GameobjectState and add them to GameobjectStateCollection.
    /// Then just saves everthing straight to json.
    /// </summary>
    public void SaveStates(string fileName)
    {
      
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        List<GameObject> saveableObjects = new List<GameObject>();

        foreach (GameObject gameObject in allObjects)
        {
            if (gameObject.CompareTag("Saveable"))
            {
                saveableObjects.Add(gameObject);
            }
        }

        Debug.Log("Found " + saveableObjects.Count + " objects with the 'Saveable' tag.");

        GameObjectStateCollection collection = new GameObjectStateCollection();

        foreach (GameObject gameObject in saveableObjects)
        {
            GameObjectState state = new GameObjectState
            {
                name = gameObject.name, 
                isActive = gameObject.activeSelf,
               
            };

            collection.gameObjectStates.Add(state);
            Debug.Log("Saving state for " + gameObject.name + ": " + (state.isActive ? "Active" : "Inactive"));
           
        }
        string json = JsonUtility.ToJson(collection, true);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName, json);

        Debug.Log("States saved for " + collection.gameObjectStates.Count + " GameObjects.");
    }

    /// <summary>
    /// Circles trhough the json file for the saved attributes.
    /// Finds ALL gameobjects in scene , filter them by Tag, like in SaveStates. The reason why: if a gameobject is set to Inactive it cant be found by only filtering by tag.
    /// Applied atrributes from jSon to gameObject.
    /// </summary>
    
    public void LoadStates(string fileName)
    {
        string filePath = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameObjectStateCollection collection = JsonUtility.FromJson<GameObjectStateCollection>(json);
            
           GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
           List<GameObject> saveableObjects = new List<GameObject>();
           
           foreach (GameObject gameObject in allObjects)
           {
               if (gameObject.CompareTag("Saveable"))
               {
                   saveableObjects.Add(gameObject);
               }
           }

            foreach (GameObject gameObject in saveableObjects)
            {
                foreach (var state in collection.gameObjectStates)
                {
                    if (state.name == gameObject.name) // Match by name
                    {
                        gameObject.SetActive(state.isActive);
                        Debug.Log("Loaded state for " + gameObject.name + ": " +
                                  (state.isActive ? "Active" : "Inactive"));
                        break;
                    }
                }
            }
           
            Debug.Log("States loaded from JSON for " + collection.gameObjectStates.Count + " GameObjects.");
        }
        else
        {
            Debug.LogWarning("No saved states found in JSON file.");
            SaveStates("gameObjectsState.json");
            return;
        }
    }


   
}