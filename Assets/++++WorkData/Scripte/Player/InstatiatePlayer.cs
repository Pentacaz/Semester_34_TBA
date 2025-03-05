using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstatiatePlayer : MonoBehaviour
{
    public GameObject playerprefab;

    private void Awake()
    {
        StartCoroutine(nameof(InstatiatePlayerPrefab));
    }

   

    IEnumerator InstatiatePlayerPrefab()
    {
        Instantiate(playerprefab);
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
    }

}
