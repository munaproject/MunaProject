using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivarObjeto : MonoBehaviour
{
    public GameObject[] objetos;
    public GameObject next;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            foreach (GameObject obj in objetos)
            {
                obj.SetActive(false);
                if(next!=null)
                {
                    next.SetActive(true);
                }
            }
        }
    }
}
