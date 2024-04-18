using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    public GameObject jugador;

    public float valueX;
    public float valueY;
    
    void Start() 
    {
        //Colocamos al jugador
        Vector2 pos = new Vector2(valueX, valueY);
        PhotonNetwork.Instantiate(jugador.name, pos, Quaternion.identity);
    }
}
