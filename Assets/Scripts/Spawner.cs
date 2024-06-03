using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    public GameObject jugadorMaster;
    public GameObject jugador;
    private GameManager gameManager;

    public float[] valueX;
    public float[] valueY;

    private bool esMaster;

    
    void Start() 
    {
        gameManager = FindObjectOfType<GameManager>();

        if (!gameManager.PosCargadas) {
            valueX[1] = gameManager.PosLille_X;
            valueY[1] = gameManager.PosLille_y;
            valueX[0] = gameManager.PosLiv_x;
            valueY[0] = gameManager.PosLiv_y;
            
            gameManager.PosCargadas = true;
        }

        if (SceneManager.GetActiveScene().name == gameManager.EscenaAnteriorAntes) { //se esta volviendo a la escena anterior
            Debug.Log("volviendo");
            Debug.Log(gameManager.PosVolverLilleAntes.x +", "+gameManager.PosVolverLilleAntes.y);
            Debug.Log(gameManager.PosVolverLivAntes.x +", "+gameManager.PosVolverLivAntes.y);
            
            valueX[0] = gameManager.PosVolverLilleAntes.x;
            valueY[0] = gameManager.PosVolverLilleAntes.y;
            valueX[1] = gameManager.PosVolverLivAntes.x;
            valueY[1] = gameManager.PosVolverLivAntes.y;

        }

        Debug.Log(gameManager.EscenaAnteriorAntes +" ? "+ SceneManager.GetActiveScene().name);

        //Colocamos al jugador
        Vector2 posMaster = new Vector2(valueX[0], valueY[0]);
        Vector2 posJugador = new Vector2(valueX[1], valueY[1]);
        //dependiendo de si el jugador es cliente master o no, controlará al personaje principal o secundario
        esMaster = PhotonNetwork.IsMasterClient;
        GameObject jugadorPrefab = esMaster ? jugadorMaster : jugador;
        Vector2 pos = esMaster ? posMaster : posJugador;
        PhotonNetwork.Instantiate(jugadorPrefab.name, pos, Quaternion.identity);
    }
}
