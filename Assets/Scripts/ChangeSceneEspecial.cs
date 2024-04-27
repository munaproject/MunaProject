using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeSceneEspecial : MonoBehaviourPunCallbacks
{
    public string nextScene;
    private bool activar;
    private bool isSceneLoading = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            activar = true;
            //si el cliente es master, ambos jugadores cambian al mismo tiempo
            //pero si no es master, hay que avisar al otro jugador (el master), que cambie de escena
            if (PhotonNetwork.IsMasterClient)
            {
                if (!isSceneLoading)
                {
                    StartCoroutine(DelayedSceneChange());
                }
            }
            else
            {
                photonView.RPC("RequestSceneChange", RpcTarget.MasterClient);
            }
        }
    }

    [PunRPC]
    void RequestSceneChange()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!isSceneLoading)
            {
                StartCoroutine(DelayedSceneChange());
            }
        }
    }
    //Corutina que esperará 0.5 segundos antes de continuar con la ejecución
    //De esta manera se evitan errores en los que el master no cargue la escena
    IEnumerator DelayedSceneChange()
    {
        isSceneLoading = true;
        yield return new WaitForSeconds(0.5f); // Adjust delay time if needed
        cambiarEscena();
        isSceneLoading = false;
    }

    void cambiarEscena()
    {
        PhotonNetwork.LoadLevel(nextScene);
    }
}