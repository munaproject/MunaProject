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
    private int posLille_x;
    private int posLille_y;
    private int posLiv_x;
    private int posLiv_y;
    

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        DontDestroyOnLoad(gameManager);
        SceneManager.LoadScene("LoginScene");
    }

    public string IdUser { get; set; }
    public string IdPartida { get; set; }
    public bool PuedeUsarLinterna { get; set; }
    public int NroBaterias { get; set; }
    public string Escena { get; set; }
    public int PosLille_X { get; set; }
    public int PosLille_y { get; set; }
    public int PosLiv_x { get; set; }
    public int PosLiv_y { get; set; }



}
