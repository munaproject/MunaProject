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
    [SerializeField] private TMP_Text dialogoTexto;
    [SerializeField] [TextArea] private string nombre;
    [SerializeField] [TextArea] private string dialogo;

    void Start() {
        nombreTexto.text = nombre;
        dialogoTexto.text = dialogo;
    }

    public void volverMenu() {
        PhotonNetwork.LoadLevel(sigEscena);
    }
}
