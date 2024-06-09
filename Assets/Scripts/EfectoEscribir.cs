using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class EfectoEscribir : MonoBehaviour
{
    [TextArea]
    public string texto;

    [Range(0.01f, 0.3f)]
    public float intervaloCaracteres;

    private string textoParcial;
    private float auxTiempo;
    public GameObject objTexto;
    public GameObject reintentar;
    public GameObject btn;
    private TextMeshProUGUI tmpText;
    private bool activado;
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        tmpText = objTexto.GetComponent<TextMeshProUGUI>();

        textoParcial = "";
        auxTiempo = 0;
        activado = false;

        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        auxTiempo += Time.deltaTime;
        while (auxTiempo >= intervaloCaracteres && textoParcial.Length < texto.Length) {
            textoParcial += texto[textoParcial.Length];
            auxTiempo -= intervaloCaracteres;
        }

        tmpText.text = textoParcial;

        if (!(textoParcial.Length < texto.Length) && !activado) {

            StartCoroutine(ActivarDespuesDeEspera());
        }

    }

    IEnumerator ActivarDespuesDeEspera()
    {
        // Espera el tiempo especificado.
        yield return new WaitForSeconds(2f);
        reintentar.SetActive(true);

        if (PhotonNetwork.IsMasterClient) {
            btn.SetActive(true);
        } 

        activado = true;
    }

    public void btnSiguiente() {
        
        PhotonNetwork.LoadLevel(gameManager.EscenaAnterior);
    }
}
