using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LilleLuzControl : MonoBehaviour
{
    bool usandoLinterna;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        usandoLinterna = false;
    }

    // Update is called once per frame
    void Update()
    {
        //linterna al teclear Q
        if (Input.GetKeyDown(KeyCode.Q) && PhotonNetwork.IsMasterClient)
        {
            usandoLinterna = !usandoLinterna;
            anim.SetBool("usingLantern", usandoLinterna);
        }
    }
}
