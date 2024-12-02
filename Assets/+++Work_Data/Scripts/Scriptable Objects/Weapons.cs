using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapons", menuName = "ScriptableObjects/Weapons", order = 3)]

public class Weapons : ScriptableObject
{
    [Header("Essentials")] 
    //public RuntimeAnimatorController animController; only needed if animated weapon or vfx is applicable. LOW prio
    public Sprite unitSprite;
    public Mesh weaponMesh;
    
    [Header("Stats")] 
    public string weaponName = "Enemy Name";
    public int weaponLevel = 1;
    public int damage = 0;
}
