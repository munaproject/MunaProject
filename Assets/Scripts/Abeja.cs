using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abeja : MonoBehaviour
{
    public float velocidad;     // Velocidad de movimiento
    private List<GameObject> jugadores; // Lista de jugadores
    private bool moviendoAPlayer; // Para saber si estamos moviéndonos hacia el jugador
    private Camera[] camarasJugadores; // Arreglo de cámaras de los jugadores
    public Vector3 posicionFin;     // Posición a la que queremos que se desplace
    private Vector3 posicionInicio;  // Posición actual
    private bool moviendoAFin;      // Para saber si vamos en dirección a la posición final o ya estamos de vuelta
    private int aux;
    private bool escondido;

    // Start is called before the first frame update
    void Start()
    {
        jugadores = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player")); // Buscar todos los jugadores por etiqueta
        camarasJugadores = new Camera[jugadores.Count]; // Inicializar el arreglo de cámaras

        for (int i = 0; i < jugadores.Count; i++)
        {
            camarasJugadores[i] = jugadores[i].GetComponentInChildren<Camera>(); // Asignar cada cámara de jugador
        }

        moviendoAPlayer = true;
        posicionInicio = transform.position;    // Nos da la posición en la que estamos
        moviendoAFin = true;
        aux = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Comprobamos si algún jugador está escondido
        escondido = false;
        foreach (var jugador in jugadores)
        {
            if (jugador.GetComponent<CharacterController>().getEsconder())
            {
                escondido = true;
                break;
            }
        }

        if (escondido)
        {
            // Si algún jugador está escondido, seguir un camino predefinido
            seguirCaminoPredefinido();
        }
        else
        {
            // Si ningún jugador está escondido, seguir al jugador
            seguirJugador();
        }
    }

    private void seguirJugador()
    {
        // Si el enemigo está en la pantalla de alguna cámara, mover la abeja hacia el jugador
        if (EstaEnPantalla())
        {
            if (aux == 0)
            {
                velocidad = velocidad + 5;
                aux++;
            }

            // Mover hacia el jugador más cercano o alguna otra lógica de selección
            GameObject jugadorObjetivo = jugadores[0]; // Simplemente tomando el primer jugador como ejemplo
            MoverEnemigo(jugadorObjetivo.transform.position);
        }
    }

    private void seguirCaminoPredefinido()
    {
        if (aux == 1)
        {
            velocidad = velocidad - 5;
            aux--;
        }

        Vector3 posicionDestino = (moviendoAFin) ? posicionFin : posicionInicio;
        transform.position = Vector3.MoveTowards(transform.position, posicionDestino, velocidad * Time.deltaTime);

        if (transform.position == posicionDestino) moviendoAFin = false;
        if (transform.position == posicionInicio) moviendoAFin = true;
    }

    private void MoverEnemigo(Vector3 destino)
    {
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
    }

    // Comprobamos que el enemigo está en la pantalla de alguna cámara
    private bool EstaEnPantalla()
    {
        foreach (var camara in camarasJugadores)
        {
            Vector3 screenPos = camara.WorldToScreenPoint(transform.position);
            if (screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height)
            {
                return true;
            }
        }
        return false;
    }
}
