using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ControlElecciones : MonoBehaviourPunCallbacks
{
    public GameObject opcUno;
    public GameObject opcDos;

    // Métodos para enviar los eventos a través de la red
    public void OnPrimeraOpcion()
    {
        photonView.RPC("ActivarPrimeraOpcion", RpcTarget.All);
    }

    public void OnSegundaOpcion()
    {
        photonView.RPC("ActivarSegundaOpcion", RpcTarget.All);
    }

    // Métodos RPC que se ejecutarán en todos los clientes
    [PunRPC]
    void ActivarPrimeraOpcion()
    {
        opcUno.SetActive(true);
        Destroy(gameObject);
    }

    [PunRPC]
    void ActivarSegundaOpcion()
    {
        opcDos.SetActive(true);
        Destroy(gameObject);
    }
}
