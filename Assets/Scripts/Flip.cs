using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    private SpriteRenderer sprite;
    private float posicionXAnterior;

    // Start is called before the first frame update
    void Start()
    {
        posicionXAnterior = transform.position.x;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x != posicionXAnterior)
        {
            // Si se mueve hacia la derecha (posición actual mayor que la anterior), no voltear.
            // Si se mueve hacia la izquierda (posición actual menor que la anterior), voltear.
            sprite.flipX = transform.position.x < posicionXAnterior;
            posicionXAnterior = transform.position.x;
        }
    }
}
