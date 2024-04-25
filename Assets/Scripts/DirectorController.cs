using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using Photon.Pun;

public class DirectorController : MonoBehaviour
{
    public GameObject[] objetosActivar;
    public GameObject[] objetosDesactivar;
    public GameObject canvas;
    public PlayableDirector director;

    public float[] tiemposDeDetencion; // Array que indica en qué segundos parará la cinematica
    private int aux = 0; // Índice para recorrer el array
    PhotonView view;

    void Start()
    {
        director.stopped += OnTimelineFinished;
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if(director != null && director.playableGraph.IsValid())
        {
            if (aux < tiemposDeDetencion.Length && director.time >= tiemposDeDetencion[aux])
            {
                director.playableGraph.GetRootPlayable(0).SetSpeed(0f); // Pausa la cinematica

            }
            if (PhotonNetwork.IsMasterClient && Input.GetButtonDown("Jump"))
            {
                siguienteDialogo();
                //hacemos que el otro jugador pase el dialogo
                view.RPC("siguienteDialogo", RpcTarget.Others);
            }
        }
    }

    [PunRPC]
    void siguienteDialogo() {
        director.playableGraph.GetRootPlayable(0).SetSpeed(1f); // Reanuda la cinematica
        aux++;
    }

    void OnTimelineFinished(PlayableDirector director)
    {
        // Activa los GameObjects especificados una vez que la cinemática haya terminado
        foreach (GameObject obj in objetosActivar)
        {
            obj.SetActive(true);
        }

        // Desactiva los GameObjects especificados una vez que la cinemática haya terminado
        foreach (GameObject obj in objetosDesactivar)
        {
            Destroy(obj);
        }

        canvas.SetActive(false);
        Destroy(director);
    }
}
