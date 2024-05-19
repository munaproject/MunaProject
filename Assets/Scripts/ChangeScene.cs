using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeScene : MonoBehaviourPunCallbacks
{
    public GameObject[] objetos;
    private bool activar;
    PhotonView view;

    void Start() {
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Jump") && activar)
        {
            foreach (GameObject obj in objetos)
            {
                obj.SetActive(true);
            }
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
