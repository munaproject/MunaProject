using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Screamer : MonoBehaviour
{
    
    public GameObject obj;
    PhotonView view;

    void Start() {
        view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            view.RPC("mostrarCanvas", RpcTarget.All);
        }
        
    }

    [PunRPC]
    void mostrarCanvas () {
        obj.SetActive(true);//activamos el guardado de partida
    }
}
