using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenu : MonoBehaviour
{
    public void OnBotonJugar()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    public void OnBotonCreditos()
    {
        SceneManager.LoadScene("CreditosScene");
    }

    public void OnBotonSalir()
    {
        Application.Quit();
    }

    public void OnBotonMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnBotonInstrucciones()
    {
        SceneManager.LoadScene("InstruccionesScene");
    }
}
