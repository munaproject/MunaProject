using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    public GameObject jugadorMaster;
    public GameObject jugador;

    public float[] valueX;
    public float[] valueY;

    private bool esMaster;
    
    void Start() 
    {
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
