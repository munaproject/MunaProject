using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esconder : MonoBehaviour
{
    private bool activar;
    private Collider2D collidedObject; // Variable para almacenar el objeto que ha colisionado

    private void Update()
    {
        // Verificar si se ha presionado el botón del mouse y si el booleano activar es verdadero
        if (Input.GetButtonDown("Jump") && activar && collidedObject != null)
        {
            // Ejecutar la función Esconder en el objeto que ha colisionado
            collidedObject.GetComponent<CharacterController>().Esconder();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        activar = true; // Activar el booleano
        collidedObject = collision; // Almacenar el objeto que ha colisionado
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activar = false;
        collision.GetComponent<CharacterController>().Esconder();
    }
}
