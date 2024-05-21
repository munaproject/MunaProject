using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PreguntaUI : MonoBehaviour
{

    //public static PreguntaUI Instance { get; private set;}

    private TextMeshProUGUI pregunta;
    private Button btnOk;
    private Button btnNo;

    private void Awake() {
        //Instance = this;

        pregunta = transform.Find("texto").GetComponent<TextMeshProUGUI>();
        btnOk = transform.Find("btnOk").GetComponent<Button>();
        btnNo = transform.Find("btnNo").GetComponent<Button>();
    }

    public void MostrarPregunta(string texto, Action siAction, Action noAction) {
        pregunta.text = texto;
        btnOk.onClick.AddListener(() => {
            gameObject.SetActive(false);
            siAction();
        });
        btnNo.onClick.AddListener(() => {
            gameObject.SetActive(false);
            noAction();
        });
    }

    public static explicit operator GameObject(PreguntaUI v)
    {
        throw new NotImplementedException();
    }
}
