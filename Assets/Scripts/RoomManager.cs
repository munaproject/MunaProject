using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public TMP_InputField crearField;
    public TMP_InputField unirseField;
    public GameObject objJugar;
    private Button btnJugar;
    private bool esMaster;


    void Start() {
        btnJugar = objJugar.GetComponent<Button>();
    }

    void Update() {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2) {
            btnJugar.interactable =true;
        } else {
            btnJugar.interactable = false;
        }
    }
    
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

    public void jugar() {
        //Empezamos el juego
        esMaster=false;
        PhotonNetwork.LoadLevel("MadreCinematica");
    }

    public override void OnJoinedRoom()
    {
        PlayerPrefs.SetInt("esMaster", true ? 1 : 0); 
        if (PhotonNetwork.IsMasterClient) {
            objJugar.SetActive(true);
        } else {
            objJugar.SetActive(false);
        }
    }
}
