using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragActivation : MonoBehaviour
{
  public GameObject initialObject;
  public GameObject fracualObject;
  public bool activeTimer = false;
 public float timer;
 private float _timerValue;
 private Collider _collider;
 public int actionRequirementId;
 

 private void Start()
 {
   _collider = this.gameObject.GetComponent<Collider>();
   _timerValue = timer;
 }

 private void Update()
  {
    if (timer <= 0)
    {
      fracualObject.SetActive(false);
     timer = _timerValue;
     activeTimer = false;
     _collider.enabled = false;
    }
    FragTimer(activeTimer);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Player") )
    {
      switch (actionRequirementId)
      {
        case 1:
          initialObject.SetActive(false);
          fracualObject.SetActive(true);
          activeTimer = true;
          break;
        case 2:
          if (other.gameObject.GetComponent<PlayerBaseController>().isDashing)
          {
            initialObject.SetActive(false);
            fracualObject.SetActive(true);
            activeTimer = true;
          }
        
          break;
       
        default:
          break;
        //other cases maybe later??
      }
   
    }
  
  }

  public void FragTimer(bool startTImer)
  {
    if (startTImer)
    {
      timer -= Time.deltaTime;
    }
   
  }
}
