using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [Header("volume")]
    [Range(0, 1)]
    
    public float masterVolume = 1;
    [Range(0, 1)]
    
    public float musicVolume = 1;
    [Range(0, 1)]
    
    public float SFXVolume = 1;
    [Range(0, 1)]
    
    private Bus masterBus;
    private Bus musicBus;
    private Bus SFXBus;
    
    private List<EventInstance> eventInstances;
    private EventInstance musicEventInstance;
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the Scene.");
        }

        instance = this;

        eventInstances = new List<EventInstance>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.music);
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        SFXBus.setVolume(SFXVolume);
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        if (musicEventInstance.isValid())
        {
            Debug.Log("Music event instance created successfully.");
            musicEventInstance.start();
        }
        else
        {
            Debug.LogError("Failed to create music event instance.");
        }
    }

// the music area is ser
    public void SetMusicArea(MusicArea area)
    {
        musicEventInstance.setParameterByName("area", (float) area);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance (EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        //stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
