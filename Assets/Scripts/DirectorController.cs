using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

public class DirectorController : MonoBehaviour
{
    public GameObject[] objetosActivar;
    public GameObject[] objetosDesactivar;
    public GameObject canvas;
    public PlayableDirector director;

    public float[] tiemposDeDetencion; // Array que indica en qué segundos parará la cinematica
    private int aux = 0; // Índice para recorrer el array

    void Start()
    {
        director.stopped += OnTimelineFinished;
    }

    void Update()
    {
        if (aux < tiemposDeDetencion.Length && director.time >= tiemposDeDetencion[aux])
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(0f); // Pausa la cinematica

        }
        if (Input.GetButtonDown("Jump"))
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(1f); // Reanuda la cinematica
            aux++;
        }
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
    }
}
