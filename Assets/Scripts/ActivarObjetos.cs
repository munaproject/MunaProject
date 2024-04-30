using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarObjetos : MonoBehaviour
{
    public GameObject[] objetos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            foreach (GameObject obj in objetos)
            {
                obj.SetActive(true);
            }
        }
    }
}
