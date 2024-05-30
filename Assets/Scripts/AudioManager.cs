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
    private int indice;
    private GameManager gameManager;

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
        gameManager = FindObjectOfType<GameManager>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        audioSource.clip = background;
        audioSource.Play();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Menu" &&
            scene.name != "Lobby"&&
            scene.name != "LoadingScene" &&
            !gameManager.MusicaCargada) {

                gameManager.MusicaCargada = true;
                background=cambios[gameManager.IndiceMusica];
                audioSource.clip = background;
                audioSource.Play();
            
        } else {
            if(scene.name == "MadreCinematica")
            {
                indice=0;
                background=cambios[indice];
                audioSource.clip = background;
                audioSource.Play();
            }
            if(scene.name == "EntradaGanarEscena" || scene.name == "EntradaPerderEscena")
            {
                indice=1;
                background=cambios[indice];
                audioSource.clip = background;
                audioSource.Play();
            }
        }
            
    }


    //--
    public string Indice { get; set; }
}