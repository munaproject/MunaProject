using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class LeerLibro : MonoBehaviour
{
    public GameObject prefabLibro;
    public string[] texto;
    private GameObject player;
    private GameObject nota;
    private bool activar;
    bool mostrando;

    void Start() {
        bool mostrando = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump") && activar && !mostrando) {
            nota = (GameObject)Instantiate(prefabLibro);
            //accedemos al texto del prefab
            nota.GetComponentInChildren<LibroUIManager>().setTexto(texto);
            
            player.GetComponent<CharacterController>().cambiarVelocidad(0);
            mostrando = true;
        }
        else if (Input.GetButtonDown("Jump") && activar && mostrando) {
            Destroy(nota);
            player.GetComponent<CharacterController>().cambiarVelocidad(5);
            mostrando = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            activar = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") activar = false;
    }
}
