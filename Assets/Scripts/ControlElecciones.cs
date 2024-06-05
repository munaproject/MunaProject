using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControlElecciones : MonoBehaviour
{
    public GameObject opcUno;
    public GameObject opcDos;

    private GameObject siguienteNombre;
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
        siguienteNombre = opcUno;
        view.RPC("siguienteObj", RpcTarget.All, siguienteNombre);
        Destroy(gameObject);
    }

    public void OnSegundaOpcion()
    {
        siguienteNombre = opcDos;
        view.RPC("siguienteObj", RpcTarget.All, siguienteNombre);
        Destroy(gameObject);
    }

    [PunRPC]
    void siguienteObj(GameObject nombreObjeto)
    {
        nombreObjeto.SetActive(true);
    }
}
