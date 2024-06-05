using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControlElecciones : MonoBehaviour
{
    public GameObject opcUno;
    public GameObject opcDos;

    private int op;
    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();

        if (view == null)
        {
            Debug.LogError("PhotonView no encontrado en el GameObject.");
        }
    }

    public void OnPrimeraOpcion()
    {
        op = 1;
        view.RPC("siguienteObj", RpcTarget.All, op);
        Destroy(gameObject);
    }

    public void OnSegundaOpcion()
    {
        op = 2;
        view.RPC("siguienteObj", RpcTarget.All, op);
        Destroy(gameObject);
    }

    [PunRPC]
    void siguienteObj(int op)
    {
        if (op == 1) opcUno.SetActive(true);
        else opcDos.SetActive(true);
    }
}
