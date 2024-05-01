using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CharacterPasos : MonoBehaviour
{
    public AudioClip audioPasos;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Steps()
    {
        audioSource.clip = audioPasos;
        audioSource.Play();

    }
}
