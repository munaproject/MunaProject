using UnityEngine;
using UnityEngine.Playables;

public class DirectorController : MonoBehaviour
{
    public PlayableDirector timeline;
    public Animator madre;
    public float[] tiemposDeDetencion; //Array que indica en que segundos parará la cinematica
    public Vector3[] posicionesPausa;
    private int aux = 0; // Índice para recorrer el array

    void Update()
    {
        if (aux < tiemposDeDetencion.Length && timeline.time >= tiemposDeDetencion[aux])
        {
            timeline.Pause();
            madre.transform.position = posicionesPausa[aux];
        }

        if (Input.GetButtonDown("Jump"))
        {
            timeline.Resume();
            aux++;
        }
    }
}
