using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
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

    public float empezarAgitar; // Segundo en el que debe empezar a agitarse
    public float duracion; // Duración del agitado en segundos

    public Camera camara; // Cámara a agitar
    private bool isShaking = false;

    void Start()
    {
        director.stopped += OnTimelineFinished;
        view = GetComponent<PhotonView>();

        // Obtener la cámara principal
        /*camara = Camera.main;
        if (camara == null)
        {
            Debug.LogError("No se encontró la cámara principal.");
        }*/
    }

    void Update()
    {
        if (director != null && director.playableGraph.IsValid())
        {
            if (aux < tiemposDeDetencion.Length && director.time >= tiemposDeDetencion[aux])
            {
                Debug.Log("Pausando la cinemática");
                director.playableGraph.GetRootPlayable(0).SetSpeed(0f); // Pausa la cinemática
            }
            if (PhotonNetwork.IsMasterClient && Input.GetButtonDown("Jump"))
            {
                siguienteDialogo();
                // Hacemos que el otro jugador pase el diálogo
                view.RPC("siguienteDialogo", RpcTarget.Others);
            }

            // Iniciar el agitado de la cámara si se cumplen las condiciones
            if (!isShaking && camara != null && director.time >= empezarAgitar)
            {
                Debug.Log("Iniciando agitado de la cámara");
                StartCoroutine(ShakeCoroutine());
                isShaking = true; // Evitar que se inicie el agitado más de una vez
            }
        }
    }

    [PunRPC]
    void siguienteDialogo()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(1f); // Reanuda la cinemática
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
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 originalPosition = camara.transform.position;

        while (elapsedTime < duracion)
        {
            float x = Random.Range(-0.1f, 0.1f);
            float y = Random.Range(-0.1f, 0.1f);

            camara.transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restaurar la posición original de la cámara después del agitado
        camara.transform.position = originalPosition;
    }
}
