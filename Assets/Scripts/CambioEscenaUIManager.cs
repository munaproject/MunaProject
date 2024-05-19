using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CambioEscenaUIManager : MonoBehaviour
{
    public string sigEscena;
    public GameObject[] objs;

    

    void Start() {
        foreach (GameObject obj in objs) {
            obj.SetActive(true);
        }
    }

    public void CambiarEscena() {
        if (PhotonNetwork.IsMasterClient) {
            //añadimos codigo para guardar
            PhotonNetwork.LoadLevel(sigEscena);
        }
    }

    public void NoCambiar() {
        gameObject.SetActive(false);
    }
}
