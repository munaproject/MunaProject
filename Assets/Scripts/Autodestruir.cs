using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Autodestruir : MonoBehaviour
{
    public float tiempo;
    
    //si despues de autodestruirse mostrará otro obj
    public GameObject siguiente;
    PhotonView view;

    void Start() {
        view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            //se autodestruye despues de x tiempo
            Destroy(gameObject, tiempo);
        }
        
    }

    private void OnDestroy()
    {
        if (siguiente != null)
        {
            view.RPC("siguienteObj", RpcTarget.All);   
        }
    }

    [PunRPC]
    void siguienteObj() {
        siguiente.SetActive(true);
    }
}
