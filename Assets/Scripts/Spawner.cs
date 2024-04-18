using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    public GameObject jugadorMaster;
    public GameObject jugador;

    public float valueX;
    public float valueY;

    private bool esMaster;
    
    void Start() 
    {
        //Colocamos al jugador
        Vector2 pos = new Vector2(valueX, valueY);
        //dependiendo de si el jugador es cliente master o no, controlará al personaje principal o secundario
        esMaster = PhotonNetwork.IsMasterClient;
        GameObject jugadorPrefab = esMaster ? jugadorMaster : jugador;
        PhotonNetwork.Instantiate(jugadorPrefab.name, pos, Quaternion.identity);
    }
}
