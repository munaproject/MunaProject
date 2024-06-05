using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class FinalUIManager : MonoBehaviour
{
    public string sigEscena;

    [SerializeField] private TMP_Text nombreTexto;
    [SerializeField] private TMP_Text finalTexto;
    [SerializeField] [TextArea] private string nombre;
    [SerializeField] [TextArea] private string final;

    void Start() {
        nombreTexto.text = nombre;
        finalTexto.text = final;
    }

    public void volverMenu() {
        StartCoroutine(VolverMenuRoutine());
    }

    private IEnumerator VolverMenuRoutine() {
        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.Disconnect();
            while (PhotonNetwork.IsConnected) {
                yield return null;
            }
        }
        SceneManager.LoadScene(sigEscena);
    }
}
