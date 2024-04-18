using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public TMP_InputField crearField;
    public TMP_InputField unirseField;
    private bool esMaster;

    
    public void crearRoom() {
        //Creamos una 'room'
        esMaster=true;
        RoomOptions options = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(crearField.text, options);
    }

    public void unirseRoom() {
        //Nos unimos a una 'room'
        esMaster=false;
        PhotonNetwork.JoinRoom(unirseField.text);
    }

    public override void OnJoinedRoom()
    {
        PlayerPrefs.SetInt("esMaster", true ? 1 : 0);
        PhotonNetwork.LoadLevel("PruebaPhoton");
    }
}
