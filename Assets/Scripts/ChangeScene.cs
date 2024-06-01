using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ChangeScene : MonoBehaviourPunCallbacks
{
    //public GameObject[] objetos;
    private bool activar;
    PhotonView view;
    public GameObject prefabPreg;
    public string sigEscena;
    public Vector2 posVolverLille;
    public Vector2 posVolverLiv;
    private GameManager gameManager;

    void Start() {
        view = GetComponent<PhotonView>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Jump") && activar)
        {
            PreguntaUI popup = Instantiate(prefabPreg).GetComponent<PreguntaUI>();
            popup.MostrarPregunta("¿Cambiar escena?", () => {

                gameManager.EscenaAnteriorAntes = gameManager.EscenaAnterior;
                gameManager.EscenaAnterior = SceneManager.GetActiveScene().name;
                gameManager.guardarPosicionesAntes();
                gameManager.PosVolverLille = posVolverLille;
                gameManager.PosVolverLiv = posVolverLiv;

                view.RPC("actualizarDatosPos", RpcTarget.Others, SceneManager.GetActiveScene().name, posVolverLille, posVolverLiv);

                Debug.Log("Escena anterior: " +gameManager.EscenaAnterior);
                if (PhotonNetwork.IsMasterClient) {
                    cambiarEscena();
                } else {
                    view.RPC("cambiarEscena", RpcTarget.Others);
                }
                Destroy(popup.gameObject);
            }, () => {
                //nada
                Destroy(popup.gameObject);
            });
        }
    }

    [PunRPC]
    void actualizarDatosPos(string esActu, Vector2 posVLille, Vector2 posVLiv) {
        gameManager.EscenaAnteriorAntes = gameManager.EscenaAnterior;
        gameManager.EscenaAnterior = esActu;
        gameManager.guardarPosicionesAntes();
        gameManager.PosVolverLille = posVLille;
        gameManager.PosVolverLiv = posVLiv;
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

    [PunRPC]
    void cambiarEscena() {
        PhotonNetwork.LoadLevel(sigEscena);
    }
}
