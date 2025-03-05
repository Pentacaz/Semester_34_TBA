using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{

    public Animator transition;
    public float transitionTime = 2f;
    public GameObject transitionCanvas;
   
    public void OnPlayButton()
    {
        transitionCanvas.SetActive(true);
        StartCoroutine(LoadScenes("Overworldtest"));
    }

    public void OnQuitButton()
    {
        Application.Quit();

    }
  
    public void GameOver()
    {

        StartCoroutine(LoadScenes("GameOver"));

    
}
    public void ReturnToStartScreen()
    {
        StartCoroutine(LoadScenes("StartScreen"));
    }

    public IEnumerator LoadScenes(string scene)
    {
        transition.SetTrigger("start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(scene);
    }

   
    


}
