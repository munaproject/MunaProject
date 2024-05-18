using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneFinCreditos : MonoBehaviour
{
    public void FinCreditos()
	{
        //comprobar si se ha terminado al verCreditos (desde el menu) o al jugar (y desconectar jugadores)
		SceneManager.LoadScene("Menu");		
	}

}
