using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject loginUI;
    public GameObject registroUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instancia existente, borrando objeto");
            Destroy(this);
        }
    }

    public void ventanaLogin()
    {
        loginUI.SetActive(true);
        registroUI.SetActive(false);
    }
    public void ventanaRegistro()
    {
        loginUI.SetActive(false);
        registroUI.SetActive(true);
    }
}
