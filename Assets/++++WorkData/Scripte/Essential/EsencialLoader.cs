using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsencialLoader : MonoBehaviour
{

  public GameObject  essentialContainer;

  private void Awake()
  {
    DontDestroyOnLoad(essentialContainer);
  }
  
}
