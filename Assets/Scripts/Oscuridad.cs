using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Oscuridad : MonoBehaviour
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
    private int tiempoEmpleado; 
    public int tiempoMax;

    private bool activar;
    private int aux;    //para comprobar por donde va el dialogo
    private bool sonidoReproducido = false; // Variable para verificar si el sonido ya se ha reproducido

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump") && activar)
        {
            if(aux==trigger)
            {
                oscuro.SetActive(true);
            }
            if(aux >= personaje.Length)
            {
                dialogoCanvas.SetActive(false);
                aux=0;  //Solo añadir esta linea si quiero que la conversacion se repita
                sonidoReproducido = false;

                tiempoEmpleado += (int) Time.deltaTime; // Aumentar el tiempo empleado

                // Cambiar la opacidad basado en el tiempo transcurrido
                if (tiempoEmpleado >= 30 && tiempoEmpleado < 60)
                {
                    oscuro.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
                    Debug.Log("30");
                }
                else if (tiempoEmpleado >= 60 && tiempoEmpleado < 90)
                {
                    oscuro.GetComponent<Image>().color = new Color(0, 0, 0, 0.75f);
                    Debug.Log("60");
                }
                else if (tiempoEmpleado >= 90)
                {
                    oscuro.GetComponent<Image>().color = new Color(0, 0, 0, 1f);
                    Debug.Log("90");
                }

                

                if (tiempoEmpleado >= tiempoMax)
                {
                    Debug.Log("Ya");
                }
            }
            else
            {
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            activar = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activar = false;
        dialogoCanvas.SetActive(false);
        sonidoReproducido = false;
    }
}
