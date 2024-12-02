using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReciever : MonoBehaviour
{
    //Enemy enemy;
   public float pushBackLenght;
    public float pushBackFactor;

    public bool tookDamage;

    

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        //enemy = GetComponent<Enemy>();
    }


    private void Update()
    {
      
        
    }
    public void GetDmg(float dmg)
    {
        //enemy.enemyHealth -= dmg;
        tookDamage = true;

        
    }





}
