using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    public GameObject master;
    public GameObject jugador;

    public float valueX;
    public float valueY;

    private int esMasterAux;
    private bool esMaster;
    
    void Start() 
    {
        //Colocamos al jugador
        Vector2 pos = new Vector2(valueX, valueY);
        esMasterInt = PlayerPrefs.GetInt("esMaster");
        esMaster = esMasterInt == 1 ? true : false;
        GameObject jugadorPrefab = esMaster ? master : jugador;
        PhotonNetwork.Instantiate(jugadorPrefab.name, pos, Quaternion.identity);
    }
}
