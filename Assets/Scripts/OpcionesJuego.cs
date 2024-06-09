using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class OpcionesJuego : MonoBehaviourPunCallbacks
{
    
    public GameObject ventana;
    private bool mostrar;
    private PhotonView view;
    private string escenaDisc;

    void Start()
    {
        mostrar = false;
        view = GetComponent<PhotonView>();
        escenaDisc = "Menu";
        DontDestroyOnLoad(gameObject);
    }

    public void mostrarVentana() {
        mostrar = !mostrar;
        ventana.SetActive(mostrar);
    }

    public void salirPartida() {
        //view.RPC("SalirPartidaTodos", RpcTarget.Others);
        //SalirPartidaTodos();
        PhotonNetwork.Disconnect();
    }

    [PunRPC]
    void SalirPartidaTodos() {
        PhotonNetwork.Disconnect();
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

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        Debug.Log("op juego"+escenaDisc);
        SceneManager.LoadScene(escenaDisc);
    }

    void HandleDisconnection()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                // If there is only one player left in the room, disconnect that player
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
            }
        }
    }

    void OnApplicationQuit()
    {
        HandleDisconnection();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            HandleDisconnection();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        HandleDisconnection();
    }

    public void cambiarACreditos() {
        escenaDisc = "Creditos";
    }
}
