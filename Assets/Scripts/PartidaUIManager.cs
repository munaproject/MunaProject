using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartidaUIManager : MonoBehaviour
{
    RoomManager roomManage;
    BbddManager bbddManage;
    //public GameObject prefabPreg;
    public GameObject btnCargarPartida;
    public GameObject btnEliminarPartida;
    public TextMeshProUGUI txtCodigo;
    private LoadAllPartidas scrollPartidas;
    
    void Start()
    {
        roomManage = FindObjectOfType<RoomManager>();
        bbddManage = FindObjectOfType<BbddManager>();
        scrollPartidas = FindObjectOfType<LoadAllPartidas>();

        //txtCodigo = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void cargarPartida() {
        roomManage.cargarPartida(txtCodigo.text); 
    }

    public void borrarPartida(string code) {
        /*
        PreguntaUI popup = Instantiate(prefabPreg).GetComponent<PreguntaUI>();
        popup.MostrarPregunta("¿Borrar partida? No se podrán recuperar los datos", () => {
            bbddManage.eliminarPartida(txtCodigo.text);
            scrollPartidas.recargarLista();
            Destroy(popup.gameObject);
        }, () => {
            //nada
            Destroy(popup.gameObject);
        });
        */
        
    }
}
