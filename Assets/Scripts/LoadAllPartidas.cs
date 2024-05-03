using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadAllPartidas : MonoBehaviour
{
    public GameObject scrollViewContent; //obj padre que contendra todas los objetos partidas
    public GameObject prefabPartida;//el objecto que se va a cargar

    private BbddManager bbdd;
    List<string> partidas;
    void Start()
    {
        bbdd = FindObjectOfType<BbddManager>();
        Debug.Log("cargando partidas...");
        loadPartidasAsync();
        
    }

    async void loadPartidasAsync() {
        partidas = await bbdd.cargarTodasPartidas();
        foreach (string code in partidas) {
            GameObject p = (GameObject)Instantiate(prefabPartida);
            TextMeshProUGUI txtCodigo = p.GetComponentInChildren<TextMeshProUGUI>();
            //ponemos el codigo
            txtCodigo.text = code;

            p.transform.localScale = transform.root.localScale;//no quitar
            p.transform.SetParent(scrollViewContent.transform);
            Debug.Log("instanciado: "+code);
        }
    }

    public void recargarLista() {
        //limpiamos la lista (solo en ui)
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
        loadPartidasAsync();//volvemos a cargar la lista
    }
}
