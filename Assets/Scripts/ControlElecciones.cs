using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControlElecciones : MonoBehaviour
{
    public GameObject opcUno;
    public GameObject opcDos;

    public GameObject desactivar;

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
        // Llamar al RPC en todos los clientes
        view.RPC("siguienteObj", RpcTarget.AllBuffered, op);
    }

    public void OnSegundaOpcion()
    {
        op = 2;
        // Llamar al RPC en todos los clientes
        view.RPC("siguienteObj", RpcTarget.AllBuffered, op);
    }

    [PunRPC]
    void siguienteObj(int op)
    {
        if (desactivar != null)
        {
            desactivar.SetActive(false);
        }
        if (op == 1) opcUno.SetActive(true);
        else opcDos.SetActive(true);

        // Destruir el objeto después de haber realizado todas las operaciones necesarias
        Destroy(gameObject);
    }
}
