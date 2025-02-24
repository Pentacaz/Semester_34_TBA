using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameData : MonoBehaviour
{
    public string playerPrefKey;

    void Start()
    {
        if (PlayerPrefs.HasKey(playerPrefKey))
        {
            print($"Keyvalue: {PlayerPrefs.GetInt(playerPrefKey)}");
            gameObject.SetActive(PlayerPrefs.GetInt(playerPrefKey) == 0);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetInt(string KeyName, int Value)
    {
        PlayerPrefs.SetInt(KeyName, Value);
        PlayerPrefs.Save();
    }
}
