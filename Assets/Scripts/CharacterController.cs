using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class CharacterController : MonoBehaviourPunCallbacks
{
    [SerializeField] public float velocidad;
    [SerializeField] private CinemachineVirtualCamera camera;
    private Rigidbody2D rig;
    private Animator anim;
    private SpriteRenderer spr;
    bool compiListo;

    PhotonView view; //variable para que cada jugador controle solo a su personaje

    Vector2 move;

    private bool velocidadManual;

    // Start is called before the first frame update
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.freezeRotation = true; //evita que lille gire, se agradece
        anim = GetComponentInChildren<Animator>();
        spr = GetComponentInChildren<SpriteRenderer>();
        velocidad=5;
        velocidadManual=false;

        view = GetComponent<PhotonView>();

        EnviarListo();
    }

    // Update is called once per frame
    void Update()
    {
        //el personaje solo se mueve si pertenece al jugador 
        if (view.IsMine) {
            Movimiento();

            camera.Priority = 1;
        }
        else
        {
            camera.Priority = 0;
        }   
    }

    void Movimiento()
    {
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

        if(!velocidadManual)
        {
                if (Input.GetKey(KeyCode.LeftShift)) //Correr
            {
                velocidad=10;
            }
            else{
                velocidad=5;
            }
        }
        
            rig.MovePosition(transform.position + (Vector3)move.normalized * Time.deltaTime * velocidad);  //Sirve para dejar al personaje mirando en la posicion que queda al parar de andar
                                                                                            //Time.deltaTime sirve para normalizar la velocidad independientemente de los frames
    }

    public void cambiarVelocidad(int num)
    {
        velocidad = num;
        if(num==5)
        {
            velocidadManual=false;
        }
        else
        {
            velocidadManual=true;
        }
        
        Debug.Log("Se cambió la velocidad a: "+velocidad);
    }

    public void Perder()
    {
        if (PhotonNetwork.IsMasterClient) {
            anim.SetBool("hasLost", true);
        }
        else
        {
            view.RPC("SyncAnimation", RpcTarget.All, true);
        }
    }

    [PunRPC]
    void cambiarEscena() {
        PhotonNetwork.LoadLevel("MeetingScene");
    }

    [PunRPC]
    void SyncAnimation(bool isFalling)
    {
        anim.SetBool("isFalling", isFalling);
    }

    public void ActivarTriggerNext()
    {
        anim.SetTrigger("Next");
    }

    void EnviarListo()
    {
        if (photonView != null)
        {
            photonView.RPC("RecibirListo", RpcTarget.Others, true);
        }
    }

    [PunRPC]
    void RecibirListo(bool estoy)
    {
        Debug.Log("Compi esta en: "+estoy);
        compiListo = true;
        
    }

    public bool estaListo() {
        return compiListo;
    }

    public void setListo(bool estado) {
        this.compiListo = false;
    }
}
