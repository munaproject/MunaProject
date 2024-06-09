using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Slime : MonoBehaviour
{
    public float velocidad;         // Velocidad de movimiento
    private float velocidadCorriendo;
    private float velocidadAux;
    public Vector3 posicionFin;     // Posición a la que queremos que se desplace
    private Vector3 posicionInicio; // Posición actual
    private bool moviendoAFin;      // Para saber si vamos en dirección a la posición final o ya estamos de vuelta
    private GameObject[] jugadores;
    private bool escondido;

    PhotonView view;

    GameObject jugadorMasCercano;
    float distanciaMasCorta;

    // Start is called before the first frame update
    void Start()
    {
        posicionInicio = transform.position; // Nos da la posición en la que estamos
        moviendoAFin = true;
        jugadores = GameObject.FindGameObjectsWithTag("Player");
        velocidadAux = velocidad;
        velocidadCorriendo = velocidad + 5;
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (jugadores.Length != 2) jugadores = GameObject.FindGameObjectsWithTag("Player");

        if (view.IsMine) { //solo el master controla el enemigo (para sincronizar)
            jugadorMasCercano = ObtenerJugadorMasCercano();
            escondido = false;
            foreach (GameObject jugador in jugadores)
            {
                if (jugador.GetComponent<CharacterController>().getEsconder())
                {
                    escondido = true;
                    break;
                }
            }

            if (escondido || distanciaMasCorta>9)
            {
                MoverEnemigo();
            }
            else
            {
                SeguirJugador();
                Debug.Log(distanciaMasCorta);
            }
        }
        
    }

    private void MoverEnemigo()
    {
        velocidad = velocidadAux;
        Vector3 posicionDestino = moviendoAFin ? posicionFin : posicionInicio;
        transform.position = Vector3.MoveTowards(transform.position, posicionDestino, velocidad * Time.deltaTime);

        if (Vector3.Distance(transform.position, posicionDestino) < 0.001f)
        {
            moviendoAFin = !moviendoAFin;
        }
    }

    private void SeguirJugador()
    {
        
        if (jugadorMasCercano != null)
        {
            velocidad = velocidadCorriendo;
            Vector3 posicionDestino = jugadorMasCercano.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, posicionDestino, velocidad * Time.deltaTime);
        }
    }

    private GameObject ObtenerJugadorMasCercano()
    {
        GameObject jugadorMasCercano = null;
        distanciaMasCorta = float.MaxValue;

        foreach (GameObject jugador in jugadores)
        {
            float distancia = Vector3.Distance(transform.position, jugador.transform.position);
            if (distancia < distanciaMasCorta)
            {
                distanciaMasCorta = distancia;
                jugadorMasCercano = jugador;
            }
        }

        return jugadorMasCercano;
    }

    private bool EstaDentroDeCamara()
    {
        foreach (GameObject jugador in jugadores)
        {
            Camera camara = jugador.GetComponentInChildren<Camera>();
            if (camara != null)
            {
                Vector3 puntoEnVista = camara.WorldToViewportPoint(transform.position);
                if (puntoEnVista.x >= 0 && puntoEnVista.x <= 1 && puntoEnVista.y >= 0 && puntoEnVista.y <= 1 && puntoEnVista.z > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
