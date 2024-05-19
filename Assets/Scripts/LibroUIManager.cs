using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LibroUIManager : MonoBehaviour
{
    
    public TextMeshProUGUI textMeshPro1;
    public TextMeshProUGUI textMeshPro2;
    public GameObject botonAtras;
    public GameObject botonAdelante;
    string[] texto;
    int indice;

    void Start()
    {
        indice = 0;
    }


    public void setTexto(string[] new_texto) {
        texto = new_texto;
        textMeshPro1.text = texto[indice];
        if (texto.Length > indice+1) textMeshPro2.text = texto[indice+1];
        if (texto.Length > 2) botonAdelante.SetActive(true);
    }

    public void PasarPagina() {
        indice += 2;
        textMeshPro2.text = ""; //en caso de que no haya otra pagina derecha
        if (texto.Length > indice) textMeshPro1.text = texto[indice];
        if (texto.Length > indice+1) textMeshPro2.text = texto[indice+1];
        
        if (texto.Length < indice+3) botonAdelante.SetActive(false);
        botonAtras.SetActive(true);
    }

    public void volverPagina() {
        indice -= 2;
        if (indice >= 0) {
            if (texto.Length > indice) textMeshPro1.text = texto[indice];
            if (texto.Length > indice+1) textMeshPro2.text = texto[indice+1];
        }
        if (indice == 0) botonAtras.SetActive(false);
        if (texto.Length > 2) botonAdelante.SetActive(true);
    }

}
