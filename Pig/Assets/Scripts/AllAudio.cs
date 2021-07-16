using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllAudio : MonoBehaviour
{
    public bool carrotsBool = false;
    public AudioClip carrotSound1;
    public AudioClip carrotSound2;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (carrotsBool)
        {
            carrotsBool = false;
            int a = Random.Range(0, 2);
            if (a == 0) audioSource.PlayOneShot(carrotSound1);
            else audioSource.PlayOneShot(carrotSound2);
        }    
    }
}
