using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    private GameObject gameManager;

    //--
    private string idUser;
    private string idPartida;
    private bool puedeUsarLinterna;
    private int nroBaterias;
    private string escena;
    private float posLille_x;
    private float posLille_y;
    private float posLiv_x;
    private float posLiv_y;
    private bool posCargadas;
    private int indiceMusica;
    private bool musicaCargada;
    private Vector2 posVolverLille;
    private Vector2 posVolverLiv;
    private Vector2 posVolverLilleAntes;
    private Vector2 posVolverLivAntes;
    private string escenaAnterior;
    private string escenaAnteriorAntes;
    

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        posCargadas = false;
        musicaCargada = false;
        DontDestroyOnLoad(gameManager);
        SceneManager.LoadScene("LoginScene");
    }


    public string IdUser { get; set; }
    public string IdPartida { get; set; }
    public bool PuedeUsarLinterna { get; set; }
    public int NroBaterias { get; set; }
    public string Escena { get; set; }
    public float PosLille_X { get; set; }
    public float PosLille_y { get; set; }
    public float PosLiv_x { get; set; }
    public float PosLiv_y { get; set; }
    public bool PosCargadas { get; set; }
    public int IndiceMusica { get; set; }
    public bool MusicaCargada { get; set; }
    public Vector2 PosVolverLille { get; set; }
    public Vector2 PosVolverLiv { get; set; }
    public Vector2 PosVolverLilleAntes { get; set; }
    public Vector2 PosVolverLivAntes { get; set; }
    public string EscenaAnterior { get; set; }
    public string EscenaAnteriorAntes { get; set; }

    public void guardarPosicionesAntes() {
        PosVolverLilleAntes = PosVolverLille;
        PosVolverLivAntes = PosVolverLiv;
    }

}
