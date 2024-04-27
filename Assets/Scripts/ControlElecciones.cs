using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlElecciones : MonoBehaviour
{
    public GameObject opcUno;
    public GameObject opcDos;

    public void OnPrimeraOpcion()
    {
        opcUno.SetActive(true);
    }

    public void OnSegundaOpcion()
    {
        opcDos.SetActive(true);
    }
}
