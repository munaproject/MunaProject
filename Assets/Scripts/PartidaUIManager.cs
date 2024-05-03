using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartidaUIManager : MonoBehaviour
{
    RoomManager roomManage;
    BbddManager bbddManage;
    public GameObject scrollViewContent;
    public GameObject btnCargarPartida;
    public GameObject btnEliminarPartida;
    public TextMeshProUGUI txtCodigo;
    private LoadAllPartidas scrollPartidas;
    
    void Start()
    {
        roomManage = FindObjectOfType<RoomManager>();
        bbddManage = FindObjectOfType<BbddManager>();
        scrollPartidas = FindObjectOfType<LoadAllPartidas>();

        txtCodigo = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void cargarPartida() {
        roomManage.cargarPartida(txtCodigo.text); 
    }

    public void borrarPartida(string code) {
        bbddManage.eliminarPartida(txtCodigo.text);
        scrollPartidas.recargarLista();
    }
}
