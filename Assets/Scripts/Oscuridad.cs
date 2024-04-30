using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
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
    public GameObject scene;
    private bool tieneLuz;
    public float intensidad;
    public GameObject next;

    private bool activar;
    private int aux;    //para comprobar por donde va el dialogo
    private bool sonidoReproducido = false; // Variable para verificar si el sonido ya se ha reproducido

    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();
        tieneLuz=false;
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

        if(master.GetComponent<CharacterController>().getLuz() || player.GetComponent<CharacterController>().getLuz())
        {
            tieneLuz=true;
        }

        if(activarEvento && !tieneLuz)
        {
            evento();
        }

        if(tieneLuz)
        {
            oscuro.GetComponentInChildren<Light2D>().intensity = intensidad;
            master.GetComponent<CharacterController>().cambiarVelocidad(5); 
            player.GetComponent<CharacterController>().cambiarVelocidad(5); 
            if(next!=null)
            {
                next.SetActive(true);
            }
        }
    }

    [PunRPC]
    void siguienteDialogo() {
        if(aux==trigger)
            {
                oscuro.SetActive(true);
                oscuro.GetComponentInChildren<Light2D>().intensity = 0.3f;
            }
            if(aux >= personaje.Length)
            {
                dialogoCanvas.SetActive(false);
                sonidoReproducido = false;

                activarEvento=true;
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

    void cambiarEscena() {
        scene.SetActive(true);
    }

    void evento()
    {
        tiempoEmpleado += Time.deltaTime; // Aumentar el tiempo empleado
        master.GetComponent<CharacterController>().cambiarVelocidad(3); 
        player.GetComponent<CharacterController>().cambiarVelocidad(3); 

        float primerTiempo = tiempoMax * 0.33f; // 33% del tiempo máximo
        float segundoTiempo = tiempoMax * 0.66f; // 66% del tiempo máximo

        // Cambiar la opacidad basado en el tiempo transcurrido
        if (tiempoEmpleado >= primerTiempo && tiempoEmpleado < segundoTiempo)
        {
            oscuro.GetComponentInChildren<Light2D>().intensity = 0.2f;
            master.GetComponent<CharacterController>().cambiarVelocidad(2); 
            player.GetComponent<CharacterController>().cambiarVelocidad(2); 
        }
        else if (tiempoEmpleado >= segundoTiempo && tiempoEmpleado < tiempoMax)
        {
            oscuro.GetComponentInChildren<Light2D>().intensity = 0.1f;
            master.GetComponent<CharacterController>().cambiarVelocidad(1); 
            player.GetComponent<CharacterController>().cambiarVelocidad(1); 
        }
        else if (tiempoEmpleado >= tiempoMax)
        {
            oscuro.GetComponentInChildren<Light2D>().intensity = 0.05f;
            master.GetComponent<CharacterController>().cambiarVelocidad(0); 
            player.GetComponent<CharacterController>().cambiarVelocidad(0); 
            master.GetComponent<CharacterController>().Perder(); 
            player.GetComponent<CharacterController>().Perder(); 
            Invoke("cambiarEscena", 4f);
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
    }
}
