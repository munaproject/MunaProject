using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterController : MonoBehaviour
{
    [SerializeField] public float velocidad = 5;

    private Rigidbody2D rig;
    private Animator anim;
    private SpriteRenderer spr;

    PhotonView view; //variable para que cada jugador controle solo a su personaje

    Vector2 move;

    // Start is called before the first frame update
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.freezeRotation = true; //evita que lille gire, se agradece
        anim = GetComponentInChildren<Animator>();
        spr = GetComponentInChildren<SpriteRenderer>();

        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        //el personaje solo se mueve si pertenece al jugador 
        if (view.IsMine) {
            move = new Vector2();
            move.x = Input.GetAxisRaw("Horizontal");
            move.y = Input.GetAxisRaw("Vertical");
            //move.Normalize();   //Se normaliza las diagonales, sin esta linea de codigo se iria mas rapido en diagonal

            if (move != Vector2.zero)   //En cuanto move sea distinto de 0, es decir, en cuanto se pulse una tecla de movimiento
            {
                anim.SetFloat("Movimiento X", move.x);
                anim.SetFloat("Movimiento Y", move.y);
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }

            if (Input.GetKey(KeyCode.LeftShift)) //Correr
            {
                velocidad=10;
            }
            else{
                velocidad=5;
            }
                rig.MovePosition(transform.position + (Vector3)move * Time.deltaTime * velocidad);  //Sirve para dejar al personaje mirando en la posicion que queda al parar de andar
                                                                                                //Time.deltaTime sirve para normalizar la velocidad independientemente de los frames

            
        }   
    }

    public void cambiarVelocidad(bool parar)
    {
        if(parar)
        {
            velocidad=0;
        }
        else
        {
            velocidad=5;
        }
    }
}
