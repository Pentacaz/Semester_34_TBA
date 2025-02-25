using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UICamera : MonoBehaviour
{
   public Camera uiCamera;

   private void Start()
   {
       
       GameObject cameraObject = GameObject.Find("UI_Camera");
       uiCamera = cameraObject.GetComponent<Camera>();
   }

   void Update()
    {


        transform.LookAt(transform.position + uiCamera.transform.rotation * Vector3.forward, uiCamera.transform.rotation * Vector3.up);

    }
}



