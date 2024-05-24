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



}
