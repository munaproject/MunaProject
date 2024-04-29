using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConseguirLinterna : MonoBehaviour
{
    public GameObject master;
    public GameObject player;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = GameObject.FindWithTag("Player");
            master = GameObject.FindWithTag("Player");

            player.GetComponent<CharacterController>().setLuz();
            master.GetComponent<CharacterController>().setLuz();
        }
    }
}
