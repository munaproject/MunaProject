using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class FinCapUIManager : MonoBehaviour
{
    public string sigEscena;
    public GameObject[] objsParaMaster;

    void Start() {
        if (PhotonNetwork.IsMasterClient) {
            foreach (GameObject obj in objsParaMaster) {
                obj.SetActive(true);
            }
        }
    }

    public void GuardarPartida() {
        if (PhotonNetwork.IsMasterClient) {
            //añadimos codigo para guardar
            PhotonNetwork.LoadLevel(sigEscena);
        }
    }

    public void NoGuardar() {
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel(sigEscena);
        }
    }
}
