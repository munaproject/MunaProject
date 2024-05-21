using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    private GameObject gameManager;

    //--
    private string IdUser;
    private string IdPartida;
    private bool puedeUsarLinterna;
    private int nroBaterias;
    

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        DontDestroyOnLoad(gameManager);
        SceneManager.LoadScene("LoginScene");
    }

}
