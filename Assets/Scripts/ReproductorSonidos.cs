using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproductorSonidos : MonoBehaviour
{
    public AudioSource audioSource; // Componente AudioSource para reproducir el sonido una vez
    public AudioClip sonidoEntrada; // Clip de sonido que quieres reproducir

    private bool sonidoReproducido = false; // Variable para verificar si el sonido ya se ha reproducido

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if (!sonidoReproducido && audioSource != null && sonidoEntrada != null)
            {
                // Reproducir el sonido de entrada si el AudioSource y el AudioClip están configurados
                audioSource.PlayOneShot(sonidoEntrada);
                sonidoReproducido = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        sonidoReproducido = false;
    }
}
