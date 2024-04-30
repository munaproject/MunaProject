using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using UnityEngine.Rendering.Universal;

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
    bool usandoLinterna;
    bool cuerpoLuzActivada;
    Light2D luzJugador;
    bool tieneLuz;
    bool puedeLuz;

    float anguloJugador;
    Quaternion rotacionLinterna;
    int anguloLinterna;
    Vector3 posicionLinterna;
    Vector2 offset; //para que la luz se situe segun el jugador y no el mapa

    // Start is called before the first frame update
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.freezeRotation = true; //evita que lille gire, se agradece
        anim = GetComponentInChildren<Animator>();
        spr = GetComponentInChildren<SpriteRenderer>();
        velocidad=5;
        velocidadManual=false;
        usandoLinterna=false;
        cuerpoLuzActivada=false;

        view = GetComponent<PhotonView>();
        luzJugador = GetComponentInChildren<Light2D>();//obtenemos la luz del jugador
        luzJugador.enabled = false;
        offset = Vector2.zero;
        tieneLuz = false;
        puedeLuz = false;

        EnviarListo();
    }

    // Update is called once per frame
    void Update()
    {
        //el personaje solo se mueve si pertenece al jugador 
        if (view.IsMine) {
            Movimiento();
            if(puedeLuz)
            {
                Linterna();
            }
            anim.SetBool("canUse", puedeLuz);

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

            if (PhotonNetwork.IsMasterClient) {
                anguloJugador = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
                rotacionLinterna = Quaternion.Euler(0f, 0f, anguloJugador-90);
                view.RPC("ColocarPosicionLinterna", RpcTarget.All, rotacionLinterna);
            }
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

    [PunRPC]
    void ColocarPosicionLinterna(Quaternion newRotacionLinterna) {
        luzJugador.transform.rotation = newRotacionLinterna;
        anguloLinterna = (int)luzJugador.transform.rotation.eulerAngles.z;
        
        //segun el angulo, la luz cambia el lugar de donde se emite
        posicionLinterna = luzJugador.transform.position;
        switch (anguloLinterna)
        {
            case 0: // arriba
                offset = new Vector2(-0.5f, -0.72f);
                break;
            case 45:
                offset = new Vector2(-0.25f, -0.78f);
                break;
            case 90: // izquierda
                offset = new Vector2(-0.43f, -0.82f);
                break;
            case 135:
                offset = new Vector2(-0.25f, -0.78f);
                break;
            case 180: // abajo
                offset = new Vector2(0.31f, -0.78f);
                break;
            case 225:
                offset = new Vector2(0.7f, -0.78f);
                break;
            case 270: // derecha
                offset = new Vector2(0.85f, -0.82f);
                break;
        }
        posicionLinterna = (Vector2)transform.position + offset;
        luzJugador.transform.position = posicionLinterna;

    }

    void Linterna()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Lille enciende la linterna
            if (PhotonNetwork.IsMasterClient) {
                usandoLinterna = !usandoLinterna;
                tieneLuz=usandoLinterna;
                anim.SetBool("usingLantern", usandoLinterna);
                view.RPC("ActualizarLinternaEnTodosLosClientes", RpcTarget.All, usandoLinterna);

            } else { //Liv activa su luz
                cuerpoLuzActivada = !cuerpoLuzActivada;
                view.RPC("ActualizarLuzCuerpoEnTodosLosClientes", RpcTarget.All, cuerpoLuzActivada);

            }
        }
    }

    [PunRPC]
    void ActualizarLinternaEnTodosLosClientes(bool nuevoEstadoLinterna)
    {
        usandoLinterna = nuevoEstadoLinterna;
        anim.SetBool("usingLantern", usandoLinterna);
        luzJugador.enabled = usandoLinterna;
    }

    [PunRPC]
    void ActualizarLuzCuerpoEnTodosLosClientes(bool nuevoEstadoLuzCuerpo) {
        luzJugador.enabled = nuevoEstadoLuzCuerpo;
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

    public void setLuz()
    {
        puedeLuz=true;
    }

    public bool getLuz()
    {
        return tieneLuz;
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
