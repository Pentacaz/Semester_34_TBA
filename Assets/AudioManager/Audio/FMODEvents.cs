using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference music{ get; private set; }
    
    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerDeath{ get; private set; }
    [field: SerializeField] public EventReference playerWoodSteps{ get; private set; }
    [field: SerializeField] public EventReference playerGrassSteps{ get; private set; }
    [field: SerializeField] public EventReference playerGravelSteps{ get; private set; }
    [field: SerializeField] public EventReference collectItem{ get; private set; }
    [field: SerializeField] public EventReference dash{ get; private set; }
    [field: SerializeField] public EventReference combat{ get; private set; }
    
    [field: Header("Bean SFX")]
    [field: SerializeField] public EventReference beanDeath{ get; private set; }
    [field: SerializeField] public EventReference beanMove{ get; private set; }

    [field: Header("Bread SFX")]
    [field: SerializeField] public EventReference breadAttack{ get; private set; }
    [field: SerializeField] public EventReference breadDeath{ get; private set; }
    [field: SerializeField] public EventReference breadGrassSteps{ get; private set; }
    [field: SerializeField] public EventReference breadGravelSteps{ get; private set; }
    
    [field: Header("Dursti SFX")]
    [field: SerializeField] public EventReference durstiDeath{ get; private set; }
    [field: SerializeField] public EventReference durstiGravelSteps{ get; private set; }
    [field: SerializeField] public EventReference durstiGrassSteps{ get; private set; }
    [field: SerializeField] public EventReference durstiShoot{ get; private set; }
    [field: SerializeField] public EventReference durstiGrowl{ get; private set; }
    
    [field: Header("Trash SFX")]
    [field: SerializeField] public EventReference canRolling{ get; private set; }
    [field: SerializeField] public EventReference cans{ get; private set; }
    [field: SerializeField] public EventReference paperMoving{ get; private set; }
    [field: SerializeField] public EventReference pebbels{ get; private set; }
    [field: SerializeField] public EventReference glassShatter{ get; private set; }

    [field: Header("Bakery SFX")]
    [field: SerializeField] public EventReference mircowave{ get; private set; }
    [field: SerializeField] public EventReference dishes{ get; private set; }
    [field: SerializeField] public EventReference doorClose{ get; private set; }
    [field: SerializeField] public EventReference doorOpen{ get; private set; }
    [field: SerializeField] public EventReference portal{ get; private set; }
    
    [field: Header("Npc SFX")]
    [field: SerializeField] public EventReference npcEating{ get; private set; }
    [field: SerializeField] public EventReference npcWalking{ get; private set; }
    
    [field: Header("Environment SFX")]
    [field: SerializeField] public EventReference rangedAttack{ get; private set; }
    [field: SerializeField] public EventReference rockShatter{ get; private set; }
    [field: SerializeField] public EventReference healingLake{ get; private set; }
    [field: SerializeField] public EventReference itemDrop{ get; private set; }




    public static FMODEvents instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events in the Scene.");
        }

        instance = this;
    }
}
