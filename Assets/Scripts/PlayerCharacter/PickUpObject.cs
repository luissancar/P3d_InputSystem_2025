using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System.Collections;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    // Transform donde se posicionará el objeto recogido (por ejemplo, una mochila)
    public Transform mochila;
    // Referencia al objeto que el personaje ha recogido
    private GameObject pickObjecct;

    private void Start()
    {
        // Inicializa la referencia del objeto recogido como nula al inicio
        pickObjecct = null;
    }

    // Método que se llama cuando el CharacterController colisiona con otro objeto
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Verifica si el objeto colisionado tiene el tag "PickCube" y si no hay un objeto ya recogido
        if (hit.collider.tag == "PickCube" && pickObjecct == null)
        {
            // Hace que el objeto colisionado sea hijo del personaje
            hit.transform.parent = this.transform;
            // Posiciona el objeto en la misma posición que la mochila
            hit.transform.position = mochila.transform.position;
            // Almacena una referencia al objeto recogido
            pickObjecct = hit.collider.gameObject;
        }
    }

    // Método para soltar el objeto recogido (se llama, por ejemplo, al pulsar una tecla como "X")
    public void SoltarObjeto() // pulsar X
    {
        RaycastHit hit;
        // Lanza un rayo hacia abajo desde la posición actual del objeto recogido
        if (Physics.Raycast(pickObjecct.transform.position, Vector3.down, out hit))
        {
            // Posiciona el objeto en la altura detectada del suelo, manteniendo las coordenadas X y Z actuales
            pickObjecct.transform.position = new Vector3(pickObjecct.transform.position.x, hit.point.y,
                pickObjecct.transform.position.z);
        }

        // Desvincula el objeto del personaje, dejándolo sin un padre en la jerarquía
        pickObjecct.transform.parent = null;
        // Inicia la corrutina para esperar antes de poder recoger otro objeto
        StartCoroutine(EsperarParaPoderCoger());
    }

    // Corrutina que espera un tiempo antes de permitir recoger otro objeto
    private IEnumerator EsperarParaPoderCoger()
    {
        // Espera 2 segundos antes de restablecer la referencia del objeto recogido
        yield return new WaitForSeconds(2f);

        // Restablece la referencia del objeto recogido a null para permitir recoger otro
        pickObjecct = null;
    }
}
