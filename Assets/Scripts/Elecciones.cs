using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Elecciones : MonoBehaviour
{
    //Referencias UI
    [SerializeField] private GameObject dialogoCanvas;
    [SerializeField] private TMP_Text personajeTexto;
    [SerializeField] private TMP_Text dialogoTexto;
    [SerializeField] private TMP_Text primeraOpcTexto;
    [SerializeField] private TMP_Text SegundaOpcTexto;
    [SerializeField] private Image retratoImagen;

    //Contenido del dialogo
    [SerializeField] private string[] personaje;
    [SerializeField] [TextArea] private string[] dialogo;
    [SerializeField] private string[] primera;
    [SerializeField] private string[] segunda;
    [SerializeField] private Sprite[] retrato;

    //SFX
    public AudioSource audioSource; // Componente AudioSource para reproducir el sonido una vez
    public AudioClip sonidoEntrada; // Clip de sonido que quieres reproducir

    private bool activar;
    private int aux;    //para comprobar por donde va el dialogo
    private bool sonidoReproducido = false; // Variable para verificar si el sonido ya se ha reproducido
    private GameObject player;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump") && activar)
        {
            if(aux >= personaje.Length)
            {
                dialogoCanvas.SetActive(false);
                player.GetComponent<CharacterController>().cambiarVelocidad(5);
                sonidoReproducido = false;
            }
            else
            {
                player.GetComponent<CharacterController>().cambiarVelocidad(0);
                dialogoCanvas.SetActive(true);
                personajeTexto.text = personaje[aux];
                dialogoTexto.text = dialogo[aux];
                retratoImagen.sprite = retrato[aux];
                if (!sonidoReproducido && audioSource != null && sonidoEntrada != null)
                {
                    // Reproducir el sonido de entrada si el AudioSource y el AudioClip están configurados
                    audioSource.PlayOneShot(sonidoEntrada);
                    sonidoReproducido = true;
                }
            }    
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            activar = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activar = false;
    }
}
