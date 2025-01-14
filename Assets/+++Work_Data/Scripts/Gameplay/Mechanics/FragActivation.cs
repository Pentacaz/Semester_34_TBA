using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragActivation : MonoBehaviour
{
  public GameObject initialObject;
  public GameObject fracualObject;
  private int timer;
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Player")
    {
      initialObject.SetActive(false);
      fracualObject.SetActive(true);
    }
  
  }
  
}
