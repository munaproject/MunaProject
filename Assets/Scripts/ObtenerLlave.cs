using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObtenerLlave : MonoBehaviourPunCallbacks
{
    private GameManager gameManager;
    PhotonView view;
    Dialogos dialogos;
    private bool activar;
    public GameObject canva;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        gameManager = FindObjectOfType<GameManager>();
        dialogos = GetComponentInChildren<Dialogos>();
        activar = false;

        if (gameManager.TieneLlave && dialogos.Terminado) Destroy(dialogos);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump") && activar)
        {
            obtenerLlave();
            view.RPC("obtenerLlave", RpcTarget.Others);
        }
        if (gameManager.TieneLlave && dialogos.Terminado && !canva.activeInHierarchy) Destroy(dialogos);
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
    }

    [PunRPC]
    void obtenerLlave() {
        gameManager.TieneLlave = true;
        dialogos.Terminado = true;
    }
}
