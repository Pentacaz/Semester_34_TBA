using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveGameData : MonoBehaviour
{//if this fails i WILL start crying
    /// <summary>
    /// basically takes the objects from the list -> translates to player prefs and then saves to json.
    /// call SaveStates("gameObjectsState.json"); to save and   LoadStates("gameObjectsState.json"); to load.
    /// </summary>
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

    // List of GameObjects to save/load
    public List<GameObject> gameObjectsToSave = new List<GameObject>();

    
    public void SaveStates(string fileName)
    {
        GameObjectStateCollection collection = new GameObjectStateCollection();

        foreach (GameObject gameObject in gameObjectsToSave)
        {
            if (gameObject != null)
            {
               
                GameObjectState state = new GameObjectState
                {
                    name = gameObject.name,
                    isActive = gameObject.activeSelf
                };

              
                PlayerPrefs.SetInt(gameObject.name + "_ActiveState", state.isActive ? 1 : 0);

               
                collection.gameObjectStates.Add(state);
            }
          
        }

       
        PlayerPrefs.Save();

        
        string json = JsonUtility.ToJson(collection, true);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName, json);

        Debug.Log("States saved for " + collection.gameObjectStates.Count + " GameObjects.");
    }


    public void LoadStates(string fileName)
    {
       
        bool loadedFromPlayerPrefs = false;
        foreach (GameObject gameObject in gameObjectsToSave)
        {
            if (gameObject != null && PlayerPrefs.HasKey(gameObject.name + "_ActiveState"))
            {
                int activeState = PlayerPrefs.GetInt(gameObject.name + "_ActiveState");
                gameObject.SetActive(activeState == 1);
                loadedFromPlayerPrefs = true;
            }
        }

        if (loadedFromPlayerPrefs)
        {
            Debug.Log("States loaded from PlayerPrefs.");
            return;
        }

        // If not found in PlayerPrefs, try to load from JSON file
        string filePath = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameObjectStateCollection collection = JsonUtility.FromJson<GameObjectStateCollection>(json);

            foreach (GameObject gameObject in gameObjectsToSave)
            {
                if (gameObject != null)
                {
                    foreach (var state in collection.gameObjectStates)
                    {
                        if (state.name == gameObject.name)
                        {
                            gameObject.SetActive(state.isActive);
                            break;
                        }
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
