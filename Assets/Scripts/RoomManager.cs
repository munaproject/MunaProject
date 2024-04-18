using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public TMP_InputField crearField;
    public TMP_InputField unirseField;

    
    public void crearRoom() {
        //Creamos una 'room'
        PhotonNetwork.CreateRoom(crearField.text);
    }

    public void unirseRoom() {
        //Nos unimos a una 'room'
        PhotonNetwork.JoinRoom(unirseField.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("PruebaPhoton");
    }
}
