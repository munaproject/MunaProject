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

    public GameObject master;
    public GameObject player;

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
    

    void Start()
    {
        view = GetComponent<PhotonView>();
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
                if (activar)
                {
                    if (PhotonNetwork.IsMasterClient) 
                    {
                        siguienteDialogo();
                        view.RPC("siguienteDialogo", RpcTarget.Others);
                    } else {
                        view.RPC("siguienteDialogo", RpcTarget.Others);
                    }
                }
                Debug.Log(modoDeActivacion);
                modoDeActivacion = opcion.interactuando;
                Debug.Log(modoDeActivacion);
            break;
        }

        

    }


    [PunRPC]
    void siguienteDialogo() {
        Debug.Log("ejecuta dialogo");
        if(aux >= personaje.Length)
        {
            dialogoCanvas.SetActive(false);
            sonidoReproducido = false;
            master.GetComponent<CharacterController>().cambiarVelocidad(5);
            player.GetComponent<CharacterController>().cambiarVelocidad(5);
            Destroy(gameObject); //cuando el dialogo termina se autodestruye
        }
        else
        {
            master.GetComponent<CharacterController>().cambiarVelocidad(0); 
            player.GetComponent<CharacterController>().cambiarVelocidad(0); 
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            activar = true;
            player = GameObject.FindWithTag("Player");
            master = GameObject.FindWithTag("Player");
        }
        Debug.Log("El jugador colisiona con el dialog");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activar = false;
    }
}
