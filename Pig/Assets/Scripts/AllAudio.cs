using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllAudio : MonoBehaviour
{
    public bool carrotsBool = false;
    public bool stepsBool = false;
    AudioSource audioSource;
    public AudioClip[] carrotSounds;
    
    public AudioClip[] stepSounds;
    public AudioSource stepsAudioSource;
    [Range(0f,1f)]
    public float stepsVolume;

    public bool refrigeratorOpenBool;
    public bool refrigeratorCloseBool;
    public AudioClip[] refrigeratorSounds;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stepsAudioSource.volume = stepsVolume;
    }


    void Update()
    {
        if (carrotsBool)
        {
            carrotsBool = false;
            audioSource.PlayOneShot(carrotSounds[Random.Range(0, carrotSounds.Length)]);
        }    
        if (stepsBool)
        {
            stepsBool = false;
            stepsAudioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
        }
        if (refrigeratorOpenBool)
        {
            refrigeratorOpenBool = false;
            audioSource.PlayOneShot(refrigeratorSounds[0]);
        }
        if (refrigeratorCloseBool)
        {
            refrigeratorCloseBool = false;
            audioSource.PlayOneShot(refrigeratorSounds[1]);
        }
    }
}
