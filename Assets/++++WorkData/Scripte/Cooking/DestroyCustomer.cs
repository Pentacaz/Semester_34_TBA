using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCustomer : MonoBehaviour
{
    
    // if the customers enter the bin, they will be deleted 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bin"))
        {
            Destroy(gameObject);
        }
    }
}
