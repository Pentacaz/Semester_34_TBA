using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ProjectileBehavior : MonoBehaviour
{
    public Projectiles projectile { get; set; }
    private void Update()
    {
       CalculateBulletHit();
    }

    public void CalculateBulletHit()
    {
        if (Physics.Raycast(new Ray(transform.position,transform.forward),out RaycastHit hit,projectile.velocity * Time.deltaTime ))
        {
            transform.position = hit.point;
            hit.collider.SendMessage("GetDamage",projectile.KineticEnergyDamage, SendMessageOptions.DontRequireReceiver);
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject,1);
            Destroy(this);
        }
        else
        {
            transform.Translate(Vector3.forward * (projectile.velocity * Time.deltaTime));
        }
    }
}
