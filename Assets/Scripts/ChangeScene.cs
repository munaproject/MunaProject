using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeScene : MonoBehaviourPunCallbacks
{
    //public GameObject[] objetos;
    private bool activar;
    PhotonView view;
    public GameObject prefabPreg;
    public string sigEscena;

    void Start() {
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Jump") && activar)
        {
            PreguntaUI popup = Instantiate(prefabPreg).GetComponent<PreguntaUI>();
            popup.MostrarPregunta("¿Cambiar escena?", () => {
                if (PhotonNetwork.IsMasterClient) {
                    cambiarEscena();
                } else {
                    view.RPC("cambiarEscena", RpcTarget.Others);
                }
                Destroy(popup.gameObject);
            }, () => {
                //nada
                Destroy(popup.gameObject);
            });
        }
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

    [PunRPC]
    void cambiarEscena() {
        PhotonNetwork.LoadLevel(sigEscena);
    }
}
