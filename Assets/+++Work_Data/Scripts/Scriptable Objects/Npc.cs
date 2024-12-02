using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Npc", menuName = "ScriptableObjects/Npc", order = 2)]
public class Npc : ScriptableObject
{
    //class for enemy stats/ configuration. Please do not edit unless asked.
    [Header("Essentials")] 
    public RuntimeAnimatorController animController;
    public Sprite npcSprite;
    public Mesh npcMesh;

    //#TODO NPC CLASSES
}

