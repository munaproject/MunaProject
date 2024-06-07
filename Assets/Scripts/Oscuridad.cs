using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;
using Photon.Pun;

public class Oscuridad : MonoBehaviourPunCallbacks
{
    // Referencias UI
    [SerializeField] private GameObject dialogoCanvas;
    [SerializeField] private TMP_Text personajeTexto;
    [SerializeField] private TMP_Text dialogoTexto;
    [SerializeField] private Image retratoImagen;

    // Contenido del dialogo
    [SerializeField] private string[] personaje;
    [SerializeField] [TextArea] private string[] dialogo;
    [SerializeField] private Sprite[] retrato;

    // SFX
    public AudioSource audioSource; // Componente AudioSource para reproducir el sonido una vez
    public AudioClip sonidoEntrada; // Clip de sonido que quieres reproducir

    // Oscuridad
    public GameObject oscuro;
    public int trigger; // Cuando se activará el evento
    private float tiempoEmpleado; 
    public float tiempoMax;
    private bool activarEvento;
    public GameObject scene;
    private bool tieneLuz;
    public float intensidad;
    public GameObject next;

    private bool activar;
    private int aux;    // Para comprobar por donde va el dialogo
    private bool sonidoReproducido = false; // Variable para verificar si el sonido ya se ha reproducido

    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();
        tieneLuz = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && activar)
        {
            siguienteDialogo();
            view.RPC("siguienteDialogo", RpcTarget.Others);
        }

        // Actualizar el estado de la luz y sincronizarlo
        bool luzActual = VerificarLuzEncendida();
        if (tieneLuz != luzActual)
        {
            tieneLuz = luzActual;
            view.RPC("ActualizarLuz", RpcTarget.All, tieneLuz);
        }

        if (activarEvento && !tieneLuz)
        {
            evento();
        }
    }

    bool VerificarLuzEncendida()
    {
        GameObject[] jugadores = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject jugador in jugadores)
        {
            if (jugador.GetComponent<CharacterController>().getLuz())
            {
                return true;
            }
        }
        return false;
    }

    [PunRPC]
    void ActualizarLuz(bool estadoLuz)
    {
        if (!estadoLuz && tieneLuz) // La luz se apagó
        {
            ReiniciarEvento();
        }
        tieneLuz = estadoLuz;
        if (tieneLuz)
        {
            PararEvento();
        }
    }

    [PunRPC]
    void siguienteDialogo() 
    {
        if (aux == trigger)
        {
            oscuro.SetActive(true);
            oscuro.GetComponentInChildren<Light2D>().intensity = 0.3f;
        }
        if (aux >= personaje.Length)
        {
            dialogoCanvas.SetActive(false);
            sonidoReproducido = false;
            view.RPC("CambiarVelocidad", RpcTarget.All, 4); 
            activarEvento = true;
            tiempoEmpleado = 0f; // Resetear el tiempo cuando se activa el evento
        }
        else
        {
            view.RPC("CambiarVelocidad", RpcTarget.All, 0); 
            dialogoCanvas.SetActive(true);
            personajeTexto.text = personaje[aux];
            dialogoTexto.text = dialogo[aux];
            retratoImagen.sprite = retrato[aux];
            aux++;
            if (!sonidoReproducido && audioSource != null && sonidoEntrada != null)
            {
                audioSource.PlayOneShot(sonidoEntrada);
                sonidoReproducido = true;
            }
        }    
    }

    void cambiarEscena() 
    {
        scene.SetActive(true);
    }

    void evento()
    {
        tiempoEmpleado += Time.deltaTime; // Aumentar el tiempo empleado

        float primerTiempo = tiempoMax * 0.33f; // 33% del tiempo máximo
        float segundoTiempo = tiempoMax * 0.66f; // 66% del tiempo máximo

        // Cambiar la opacidad basado en el tiempo transcurrido
        if (tiempoEmpleado >= primerTiempo && tiempoEmpleado < segundoTiempo)
        {
            oscuro.GetComponentInChildren<Light2D>().intensity = intensidad / 3 * 2;
            view.RPC("CambiarVelocidad", RpcTarget.All, 2);
        }
        else if (tiempoEmpleado >= segundoTiempo && tiempoEmpleado < tiempoMax)
        {
            oscuro.GetComponentInChildren<Light2D>().intensity = intensidad / 3;
            view.RPC("CambiarVelocidad", RpcTarget.All, 1);
        }
        else if (tiempoEmpleado >= tiempoMax)
        {
            oscuro.GetComponentInChildren<Light2D>().intensity = intensidad / 3 / 3;
            view.RPC("CambiarVelocidad", RpcTarget.All, 0);
            view.RPC("Perder", RpcTarget.All);
            Invoke("cambiarEscena", 4f);
        }
    }

    [PunRPC]
    void ReiniciarEvento()
    {
        tiempoEmpleado = 0f;
        activarEvento = false;
        // Restablecer cualquier otra variable o estado necesario aquí
        oscuro.GetComponentInChildren<Light2D>().intensity = intensidad / 3 * 2;; // Volver a la intensidad inicial del evento
        view.RPC("CambiarVelocidad", RpcTarget.All, 3);
        activarEvento = true; // Reactivar el evento inmediatamente después de reiniciarlo
    }

    [PunRPC]
    void PararEvento()
    {
        activarEvento = false; // Asegurarse de que el evento se detenga
        oscuro.GetComponentInChildren<Light2D>().intensity = intensidad;
        view.RPC("CambiarVelocidad", RpcTarget.All, 5);
        if (next != null)
        {
            next.SetActive(true);
        }
    }

    [PunRPC]
    void CambiarVelocidad(int velocidad)
    {
        GameObject[] jugadores = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject jugador in jugadores)
        {
            jugador.GetComponent<CharacterController>().cambiarVelocidad(velocidad); 
        }
    }

    [PunRPC]
    void Perder()
    {
        GameObject[] jugadores = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject jugador in jugadores)
        {
            jugador.GetComponent<CharacterController>().Perder(); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            activar = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activar = false;
    }
}
