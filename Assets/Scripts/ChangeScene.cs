using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeScene : MonoBehaviourPunCallbacks
{
    public string nextScene;
    private bool activar;
    PhotonView view;

    void Start() {
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Jump") && activar)
        {
            //si el cliente es master, ambos jugadores cambian al mismo tiempo
            //pero si no es master, hay que avisar al otro jugador (el master), que cambie de escena
            if (PhotonNetwork.IsMasterClient) {
                cambiarEscena();
            } else {
                view.RPC("cambiarEscena", RpcTarget.Others);
            }
        }
    }

    [PunRPC]
    void cambiarEscena() {
        PhotonNetwork.LoadLevel(nextScene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            activar = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activar = false;
    }
}
