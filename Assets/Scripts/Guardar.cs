using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guardar : MonoBehaviour
{

    public GameObject prefabPreg;
    private bool activar;

    BbddManager bbddManager;
    GameManager gameManager;
    private CharacterController[] personajes;

    void Start()
    {
        bbddManager = FindObjectOfType<BbddManager>();
        gameManager = FindObjectOfType<GameManager>();

    }

    void Update()
    {
        if(Input.GetButtonDown("Jump") && activar)
        {
            PreguntaUI popup = Instantiate(prefabPreg).GetComponent<PreguntaUI>();
            popup.MostrarPregunta("¿Guardar partida?", () => {
                //--
                personajes = FindObjectsOfType<CharacterController>();
                bbddManager.guardarDatos(
                    gameManager.IdPartida, 
                    SceneManager.GetActiveScene().name, 
                    gameManager.IndiceMusica,
                    personajes[0].transform.position,
                    personajes[1].transform.position);

                //--
                Destroy(popup.gameObject);
            }, () => {
                //nada
                Destroy(popup.gameObject);
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            activar = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activar = false;
    }
}
