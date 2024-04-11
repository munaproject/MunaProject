using UnityEngine;
using UnityEngine.Playables;

public class DirectorController : MonoBehaviour
{
    public PlayableDirector timeline;
    public Animator animator;
    public float[] tiemposDeDetencion; // Array que indica en qué segundos parará la cinematica
    public Vector3[] posicionesPausa;
    private int aux = 0; // Índice para recorrer el array

    /*void Start()
    {
        animator = GetComponent<Animator>();
    }*/

    void Update()
    {
        if (aux < tiemposDeDetencion.Length && timeline.time >= tiemposDeDetencion[aux])
        {
            timeline.playableGraph.GetRootPlayable(0).SetSpeed(0f); // Pausa la cinematica
            animator.Play("Madre");
        }

        if (Input.GetButtonDown("Jump"))
        {
            timeline.playableGraph.GetRootPlayable(0).SetSpeed(1f); // Reanuda la cinematica
            aux++;
        }
    }
}
