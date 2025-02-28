using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveGameData : MonoBehaviour
{
    //if this fails i WILL start crying
    /// <summary>
    /// basically takes the objects from the list -> translates to player prefs and then saves to json.
    /// call SaveStates("gameObjectsState.json"); to save and   LoadStates("gameObjectsState.json"); to load.
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
                isActive = gameObject.activeSelf
            };

            collection.gameObjectStates.Add(state);
            Debug.Log("Saving state for " + gameObject.name + ": " + (state.isActive ? "Active" : "Inactive"));
        }

        string json = JsonUtility.ToJson(collection, true);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName, json);

        Debug.Log("States saved for " + collection.gameObjectStates.Count + " GameObjects.");
    }

    
    
    public void LoadStates(string fileName)
    {
        string filePath = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameObjectStateCollection collection = JsonUtility.FromJson<GameObjectStateCollection>(json);


           // GameObject[] saveableObjects = GameObject.FindGameObjectsWithTag("Saveable");
           
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
        }
    }
}