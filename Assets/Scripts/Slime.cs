using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Slime : MonoBehaviourPun, IPunObservable
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
    private Vector3 networkPosition;
    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
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
        if (!view.IsMine)
        {
            transform.position = Vector3.MoveTowards(transform.position, networkPosition, Time.deltaTime * velocidad);
            return;
        }

        escondido = false;
        foreach (var jugador in jugadores)
        {
            if (jugador.GetComponent<CharacterController>().getEsconder())
            {
                escondido = true;
                break;
            }
        }

        if (escondido || !EstaEnPantalla())
        {
            seguirCaminoPredefinido();
        }
        else
        {
            seguirJugador();
        }
    }

    [PunRPC]
    private void seguirCaminoPredefinido()
    {
        if (aux == 1)
        {
            velocidad = velocidad - 5;
            aux--;
        }

        Vector3 posicionDestino = (moviendoAFin) ? posicionFin : posicionInicio;
        transform.position = Vector3.MoveTowards(transform.position, posicionDestino, velocidad * Time.deltaTime);

        if (Vector3.Distance(transform.position, posicionDestino) < 0.01f)
        {
            moviendoAFin = !moviendoAFin;
        }
    }

    [PunRPC]
    private void seguirJugador()
    {
        if (EstaEnPantalla())
        {
            if (aux == 0)
            {
                velocidad = velocidad + 5;
                aux++;
            }

            GameObject jugadorObjetivo = null;
            float distanciaMinima = Mathf.Infinity;

            foreach (var jugador in jugadores)
            {
                float distancia = Vector3.Distance(transform.position, jugador.transform.position);
                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    jugadorObjetivo = jugador;
                }
            }

            if (jugadorObjetivo != null)
            {
                MoverEnemigo(jugadorObjetivo.transform.position);
            }
        }
    }

    private void MoverEnemigo(Vector3 destino)
    {
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
    }

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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
        }
    }
}
