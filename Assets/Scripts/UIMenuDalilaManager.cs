using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UIMenuDalilaManager : MonoBehaviour
{
    public GameObject btn;

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) {
            btn.SetActive(false);
        }   
    }

    public void btnSiguiente() {
        //siguiente escena (falta crear la escena)
        PhotonNetwork.LoadLevel("CapituloDos");
    }

}
