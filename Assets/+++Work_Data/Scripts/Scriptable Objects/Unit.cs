using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Unit", order = 1)]
public class Unit : ScriptableObject
{
    //class for enemy stats/ configuration. Please do not edit unless asked.
    [Header("Essentials")] 
    public RuntimeAnimatorController animController;
    public Sprite unitSprite;
    public Mesh UnitMesh;
    
   [Header("Stats")] 
    public string unitName = "Enemy Name";
    public int unitLevel = 1;
    public int defense;
    public int damage = 0;
    public int maxHp = 0;
}
