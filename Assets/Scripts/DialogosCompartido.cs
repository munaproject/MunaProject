using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class DialogosCompartido : MonoBehaviour
{
    public opcion modoDeActivacion;
    public enum opcion
    {
        interactuando,
        colisionando
        
    }

    //public GameObject master;
    //public GameObject player;

    private CharacterController[] personajes;

    //Referencias UI
    [SerializeField] private GameObject dialogoCanvas;
    [SerializeField] private TMP_Text personajeTexto;
    [SerializeField] private TMP_Text dialogoTexto;
    [SerializeField] private Image retratoImagen;

    //Contenido del dialogo
    [SerializeField] private string[] personaje;
    [SerializeField] [TextArea] private string[] dialogo;
    [SerializeField] private Sprite[] retrato;

    //SFX
    public AudioSource audioSource; // Componente AudioSource para reproducir el sonido una vez
    public AudioClip sonidoEntrada; // Clip de sonido que quieres reproducir

    //--
    private bool activar;
    private int aux;    //para comprobar por donde va el dialogo
    private bool sonidoReproducido = false; // Variable para verificar si el sonido ya se ha reproducido

    PhotonView view;
    private bool autodestruir;
    public GameObject next;
    

    void Start()
    {
        view = GetComponent<PhotonView>();
        autodestruir = false;

        personajes = FindObjectsOfType<CharacterController>();
    }

    void Update()
    {
        switch (modoDeActivacion) {
            case opcion.interactuando: //al interactuar con espacio
                if (Input.GetButtonDown("Jump") && activar)
                {
                    if (PhotonNetwork.IsMasterClient) 
                    {
                        siguienteDialogo();
                        view.RPC("siguienteDialogo", RpcTarget.Others);
                    } else {
                        view.RPC("siguienteDialogo", RpcTarget.Others);
                    }
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

    }

    [PunRPC]
    void siguienteDialogo() {
        if(aux >= personaje.Length)
        {
            dialogoCanvas.SetActive(false);
            sonidoReproducido = false;
            //master.GetComponent<CharacterController>().cambiarVelocidad(5);
            //player.GetComponent<CharacterController>().cambiarVelocidad(5);
            personajes[0].cambiarVelocidad(5);
            personajes[1].cambiarVelocidad(5);
            if (autodestruir) Destroy(gameObject); //cuando el dialogo termina se autodestruye
            if(next!=null)
            {
                next.SetActive(true);
            }
        }
        else
        {

            mostrarCanvasDialogo();
        }    
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
            //player = GameObject.FindWithTag("Player");
            //master = GameObject.FindWithTag("Player");
            //detenemos a los jugadores si el modo de activacion es colisionando
            if (modoDeActivacion == opcion.colisionando) {
                //master.GetComponent<CharacterController>().cambiarVelocidad(0); 
                //player.GetComponent<CharacterController>().cambiarVelocidad(0);
                personajes[0].cambiarVelocidad(0);
                personajes[1].cambiarVelocidad(0); 
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (modoDeActivacion == opcion.interactuando) activar = false;
    }
}
