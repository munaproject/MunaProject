using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeScene : MonoBehaviourPunCallbacks
{
    public string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Cambio");
            if(Input.GetButtonDown("Jump"))
            {
                PhotonNetwork.LoadLevel(nextScene);
            }
        }
    }
}
