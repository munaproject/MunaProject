using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public AudioClip background;
    public AudioClip[] cambios;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        audioSource.clip = background;
        audioSource.Play();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MadreCinematica")
        {
            background=cambios[0];
            audioSource.clip = background;
            audioSource.Play();
        }
        if(scene.name == "EntradaGanarEscena" || scene.name == "EntradaPerderEscena")
        {
            background=cambios[1];
            audioSource.clip = background;
            audioSource.Play();
        }
    }
}