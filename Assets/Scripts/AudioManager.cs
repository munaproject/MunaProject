using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public AudioClip background;
    public AudioClip[] cambios;
    public float[] volumes;

    public static AudioManager instance;
    private int indice;
    private GameManager gameManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        audioSource.clip = background;
        audioSource.volume = volumes[0]; // Set initial volume
        audioSource.Play();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Menu" &&
            scene.name != "Lobby" &&
            scene.name != "LoadingScene" &&
            !gameManager.MusicaCargada &&
            gameManager.CargarTodo)
        {
            gameManager.MusicaCargada = true;
            indice = gameManager.IndiceMusica;
            background = cambios[indice];
            audioSource.clip = background;
            audioSource.volume = volumes[indice]; // Set volume based on index
            audioSource.Play();
        }
        else
        {
            if (scene.name == "MadreCinematica")
            {
                indice = 0;
                background = cambios[indice];
                audioSource.clip = background;
                audioSource.volume = volumes[indice]; // Set volume based on index
                audioSource.Play();
            }
            if (scene.name == "EntradaGanarEscena" || scene.name == "EntradaPerderEscena")
            {
                indice = 1;
                background = cambios[indice];
                audioSource.clip = background;
                audioSource.volume = volumes[indice]; // Set volume based on index
                audioSource.Play();
            }
            if (scene.name == "CapituloDos")
            {
                indice = 2;
                background = cambios[indice];
                audioSource.clip = background;
                audioSource.volume = volumes[indice]; // Set volume based on index
                audioSource.Play();
            }
        }
    }

    public string Indice { get; set; }
}
