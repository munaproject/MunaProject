using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Photon.Pun;

public class OscuridadSinDialogo : MonoBehaviourPunCallbacks
{
    // SFX
    public AudioSource audioSource; // Componente AudioSource para reproducir el sonido una vez
    public AudioClip sonidoEntrada; // Clip de sonido que quieres reproducir

    // Oscuridad
    public GameObject oscuro;
    private float tiempoEmpleado; 
    public float tiempoMax;
    private bool activarEvento;
    public GameObject scene;
    private bool tieneLuz;
    public float intensidad;
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
        oscuro.GetComponentInChildren<Light2D>().intensity = intensidad; // Volver a la intensidad inicial del evento
        view.RPC("CambiarVelocidad", RpcTarget.All, 3);
        activarEvento = true; // Reactivar el evento inmediatamente después de reiniciarlo
    }

    [PunRPC]
    void PararEvento()
    {
        activarEvento = false; // Asegurarse de que el evento se detenga
        oscuro.GetComponentInChildren<Light2D>().intensity = intensidad;
        view.RPC("CambiarVelocidad", RpcTarget.All, 5);
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
