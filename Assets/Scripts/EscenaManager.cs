using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EscenaManager : MonoBehaviour
{

    public GameObject[] jugadores;
    CharacterController charControl;
    public PlayableDirector director;
    bool listos = true;

    void Update()
    {
        foreach (GameObject jugador in jugadores)
        {
            charControl = jugador.GetComponent<CharacterController>();
            if (charControl != null && !charControl.estaListo())
            {
                listos = false;
                break;
            }
        }
        if (listos)
        {
            listos = false;
            //quitamos el estado de listos para que la cinematica no se repita infinitamente
            foreach (GameObject jugador in jugadores) {
                charControl = jugador.GetComponent<CharacterController>();
                charControl.setListo(false);
                Debug.Log("Debe ser falso: "+charControl.estaListo());
            }
            director.Play();//si ambos jugadores estan listos, empieza la cinematica
        }
    }
}
