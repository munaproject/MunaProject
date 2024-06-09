using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class QuitarCadena : MonoBehaviour
{

    private bool activar;
    private GameManager gameManager;
    private PhotonView view;
    public GameObject next;

    // Start is called before the first frame update
    void Start()
    {
        activar = false;
        gameManager = FindObjectOfType<GameManager>();
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump") && activar && gameManager.TieneLlave) {
            next.SetActive(true);
            view.RPC("autodestruir", RpcTarget.All);

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
    }

    [PunRPC]
    void autodestruir() {
        Destroy(gameObject);
    }
}
