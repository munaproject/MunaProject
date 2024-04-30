using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestruirNoPlayer : MonoBehaviour
{
    private BoxCollider2D col;
    public GameObject caja; //el objeto que se destruira

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        col.isTrigger = false;
        Destroy(caja);
    }


}
