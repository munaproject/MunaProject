using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public TMP_InputField crearField;
    public TMP_InputField unirseField;
    public GameObject objJugar;
    public GameObject objTexto;
    private Button btnJugar;
    private bool esMaster;

    [Header("Ventanas")]
    public GameObject panelElegir;
    public GameObject panelCrear;
    public GameObject panelUnirse;
    public GameObject panelEspera;

    [Header("Sig Escena")]
    public string escena;

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

    public void verVentanaCrear() {
        panelElegir.SetActive(false);
        panelCrear.SetActive(true);
    }

    public void verVentanaUnirse() {
        panelElegir.SetActive(false);
        panelUnirse.SetActive(true);
    }

    public void verVentanaEspera() {
        panelCrear.SetActive(false);
        panelUnirse.SetActive(false);
        panelEspera.SetActive(true);
    }

    public void volverPanelUnirse() {
        panelEspera.SetActive(false);
        panelUnirse.SetActive(true);
    }

    public void volverPanelCrear() {
        panelEspera.SetActive(false);
        panelCrear.SetActive(true);
    }

    public void salirEspera() {
        PhotonNetwork.LeaveRoom();
        if (PhotonNetwork.IsMasterClient) {
            volverPanelCrear();
        } else {
            volverPanelUnirse();
        }
    }

    public void volverPanelElegir() {
        panelCrear.SetActive(false);
        panelUnirse.SetActive(false);
        panelElegir.SetActive(true);
    }

    public void volverMenu() {
        //PhotonNetwork.LeaveRoom();
        //PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
    }
    
    public void crearRoom() {
        //Creamos una 'room'
        esMaster=true;
        RoomOptions options = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(crearField.text, options);
        crearField.text = "";
    }

    public void unirseRoom() {
        //Nos unimos a una 'room'
        esMaster=false;
        PhotonNetwork.JoinRoom(unirseField.text);
        unirseField.text = "";
    }

    public void jugar() {
        Debug.Log("clic jugar");
        //Empezamos el juego
        esMaster=false;
        PhotonNetwork.LoadLevel(escena);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("se unio a la room");
        verVentanaEspera();
        PlayerPrefs.SetInt("esMaster", true ? 1 : 0); 
        if (PhotonNetwork.IsMasterClient) {
            objJugar.SetActive(true);
            objTexto.SetActive(false);
        } else {
            objJugar.SetActive(false);
            objTexto.SetActive(true);
        }
    }

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.Log("Motivo de desconexion: " + cause);

        switch (cause)
        {
            case DisconnectCause.DisconnectByClientLogic:
                Debug.Log("desconectado por cliente");
                break;

            default:
                Debug.Log("Desoncexion por un error (ver causas)...");
                break;
        }

        SceneManager.LoadScene("Menu");
    }
}
