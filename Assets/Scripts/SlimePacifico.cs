using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SlimePacifico : MonoBehaviour
{
    public float velocidad;         //Velocidad de movimiento
    public Vector3 posicionFin;     //Posicion a la que queremos que se desplace
    private Vector3 posicionInicio;  //Posicion actual
    private bool moviendoAFin;      //Para saber si vamos en direccion a la posicion final o ya estamos de vuelta
    private float velocidadAux;

    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        posicionInicio = transform.position;    //Nos da la posicion en la que estamos
        moviendoAFin = true;
        velocidadAux=velocidad;
    }

    // Update is called once per frame
    void Update()
    {
        MoverEnemigo();
    }

    private void MoverEnemigo()
    {
        Vector3 posicionDestino = (moviendoAFin) ? posicionFin : posicionInicio;
        transform.position = Vector3.MoveTowards(transform.position, posicionDestino, velocidad * Time.deltaTime);

        if (transform.position == posicionDestino) moviendoAFin = false;
        if (transform.position == posicionInicio) moviendoAFin = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            velocidad=0;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            velocidad=velocidadAux;
        }
    }
}