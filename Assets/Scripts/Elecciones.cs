using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Elecciones : MonoBehaviour
{
    public opcion modoDeActivacion;
    public enum opcion
    {
        interactuando,
        colisionando
        
    }

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

    PhotonView view;
    private bool activar;
    private bool autodestruir;
    private int aux;    //para comprobar por donde va el dialogo
    private bool sonidoReproducido = false; // Variable para verificar si el sonido ya se ha reproducido
    private GameObject player;

    void Start()
    {
        view = GetComponent<PhotonView>();
        autodestruir = false;

    }

    // Update is called once per frame
    void Update()
    {
        switch (modoDeActivacion) {
            case opcion.interactuando: //al interactuar con espacio
                if (Input.GetButtonDown("Jump") && activar)
                {
                    mostrarCanvasDialogo();
                    view.RPC("mostrarCanvasDialogo", RpcTarget.Others);
                }
            break;
            case opcion.colisionando://al tocar, se activa automaticamente
                //despues de ejecutarse una vez, se reasigna la variable para que entre en el switch de 'interactuando'
                if (activar & aux==0) //se comprueba el aux para que se ejecute solo en el primer dialogo
                {
                    view.RPC("mostrarCanvasDialogo", RpcTarget.All);
                    autodestruir = true;
                    modoDeActivacion = opcion.interactuando;
                }
            break;
        }
        /*if(Input.GetButtonDown("Jump") && activar)
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
        }*/
    }

    [PunRPC]
    void mostrarCanvasDialogo() {
        
        dialogoCanvas.SetActive(true);
        personajeTexto.text = personaje[aux];
        dialogoTexto.text = dialogo[aux];
        retratoImagen.sprite = retrato[aux];
        aux++;

        if (!sonidoReproducido && audioSource != null && sonidoEntrada != null)
        {
            // Reproducir el sonido de entrada si el AudioSource y el AudioClip están configurados
            audioSource.PlayOneShot(sonidoEntrada);
            sonidoReproducido = true;
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
