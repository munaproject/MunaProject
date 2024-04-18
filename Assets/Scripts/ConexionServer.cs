using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Firebase;
using UnityEngine.SceneManagement;

public class ConexionServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        //Nos conectamos a la configuracino base de photon
        PhotonNetwork.ConnectUsingSettings();
        //Con eso ya tenenmos la conexion lista
    }

    public override void OnConnectedToMaster()
    {
        //hacemos que los jugadores puedan conectarse a una misma sala
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void btnConectar() 
    {
        //establecemos un max de 4 jugadores
        RoomOptions opciones = new RoomOptions() { MaxPlayers = 2 };

        PhotonNetwork.JoinOrCreateRoom("room1", opciones, TypedLobby.Default);
    }

}
