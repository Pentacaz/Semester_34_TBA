using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
  public List <Transform> enemySpawnPoints;
  public List<GameObject> enemy;
  public int rounds;
  
  
  public void RespawnEnemies()
   {
       if (rounds > 0)
       {
           Instantiate();
       }
   }
}
