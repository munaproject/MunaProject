using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogos : MonoBehaviour
{
    //Referencias UI
    [SerializeField] private GameObject dialogoCanvas;
    [SerializeField] private TMP_Text personajeTexto;
    [SerializeField] private TMP_Text dialogoTexto;
    [SerializeField] private Image retratoImagen;

    //Contenido del dialogo
    [SerializeField] private string[] personaje;
    [SerializeField] [TextArea] private string[] dialogo;
    [SerializeField] private Sprite[] retrato;

    private bool activar;
    private int aux;    //para comprobar por donde va el dialogo

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Interact") && activar)
        {
            if(aux >= personaje.Length)
            {
                dialogoCanvas.SetActive(false);
                aux=0;  //Solo añadir esta linea si quiero que la conversacion se repita
            }
            else
            {
                dialogoCanvas.SetActive(true);
                personajeTexto.text = personaje[aux];
                dialogoTexto.text = dialogo[aux];
                retratoImagen.sprite = retrato[aux];
                aux++;
            }    
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            activar == true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activar == false;
        dialogoCanvas.SetActive(false);
    }
}
