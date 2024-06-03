using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class FinCapUIManager : MonoBehaviour
{
    public string sigEscena;
    public GameObject[] objsParaMaster;
    BbddManager bbddManager;
    GameManager gameManager;
    private CharacterController[] personajes;

    void Start() {
        bbddManager = FindObjectOfType<BbddManager>();
        gameManager = FindObjectOfType<GameManager>();
        if (PhotonNetwork.IsMasterClient) {
            foreach (GameObject obj in objsParaMaster) {
                obj.SetActive(true);
            }
        }
    }

    public void GuardarPartida() {
        if (PhotonNetwork.IsMasterClient) {
            personajes = FindObjectsOfType<CharacterController>();
            bbddManager.guardarDatos(
                gameManager.IdPartida, 
                sigEscena, 
                gameManager.IndiceMusica,
                personajes[0].transform.position,
                personajes[1].transform.position,
                false);
            
            PhotonNetwork.LoadLevel(sigEscena);
        }
    }

    public void NoGuardar() {
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel(sigEscena);
        }
    }
}
