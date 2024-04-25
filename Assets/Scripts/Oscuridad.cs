using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Oscuridad : MonoBehaviourPunCallbacks
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

    //SFX
    public AudioSource audioSource; // Componente AudioSource para reproducir el sonido una vez
    public AudioClip sonidoEntrada; // Clip de sonido que quieres reproducir

    //Oscuridad
    public GameObject oscuro;
    public int trigger; //cuando se activará el evento
    private float tiempoEmpleado; 
    public float tiempoMax;
    private bool activarEvento;
    public GameObject master;
    public GameObject player;

    private bool activar;
    private int aux;    //para comprobar por donde va el dialogo
    private bool sonidoReproducido = false; // Variable para verificar si el sonido ya se ha reproducido

    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
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

        if(activarEvento)
        {
            tiempoEmpleado += Time.deltaTime; // Aumentar el tiempo empleado
            master.GetComponent<CharacterController>().cambiarVelocidad(3); 
            player.GetComponent<CharacterController>().cambiarVelocidad(3); 

            float primerTiempo = tiempoMax * 0.33f; // 33% del tiempo máximo
            float segundoTiempo = tiempoMax * 0.66f; // 66% del tiempo máximo

            // Cambiar la opacidad basado en el tiempo transcurrido
            if (tiempoEmpleado >= primerTiempo && tiempoEmpleado < segundoTiempo)
            {
                oscuro.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.6f);
                master.GetComponent<CharacterController>().cambiarVelocidad(2); 
                player.GetComponent<CharacterController>().cambiarVelocidad(2); 
            }
            else if (tiempoEmpleado >= segundoTiempo && tiempoEmpleado < tiempoMax)
            {
                oscuro.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.75f);
                master.GetComponent<CharacterController>().cambiarVelocidad(1); 
                player.GetComponent<CharacterController>().cambiarVelocidad(1); 
            }
            else if (tiempoEmpleado >= tiempoMax)
            {
                oscuro.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.95f);
                master.GetComponent<CharacterController>().cambiarVelocidad(0); 
                player.GetComponent<CharacterController>().cambiarVelocidad(0); 
                master.GetComponent<CharacterController>().Perder(); 
            }
        }
    }

    [PunRPC]
    void siguienteDialogo() {
        if(aux==trigger)
            {
                oscuro.SetActive(true);
            }
            if(aux >= personaje.Length)
            {
                dialogoCanvas.SetActive(false);
                sonidoReproducido = false;

                activarEvento=true;
                player = GameObject.FindWithTag("Player");
                master = GameObject.FindWithTag("Player");
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
        }
    }
}
