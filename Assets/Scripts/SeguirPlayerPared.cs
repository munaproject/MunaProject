using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeguirPlayerPared : MonoBehaviour
{
    private GameObject jugador;
    private BoxCollider2D col;

    Vector3 posJugIni;
    Vector3 posJugFin;

    private Rigidbody2D jugadorRigidbody;

    void Start () {
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (jugadorRigidbody != null) {
            posJugFin = jugador.transform.position;
            //se sigue al jugador cuando se mueve por el eje de las x (derecha)
            if (posJugFin.x - posJugIni.x >0) {
                transform.position = transform.position + posJugFin - posJugIni;
                posJugIni = posJugFin;
            }
                
        }
    }

    private void OnTriggerExit2D(Collider2D obj)
    {
        //cuando el jugador sale del objeto, se activa el collider y empieza a seguir al jugador
        jugador = obj.gameObject;
        col.isTrigger = false;
        posJugIni = jugador.transform.position;
        jugadorRigidbody = jugador.GetComponent<Rigidbody2D>();
    }    

}
