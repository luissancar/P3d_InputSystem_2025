using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esta clase permite mover objetos Rigidbody en Unity cuando el personaje colisiona con ellos
public class MoverRigiBody : MonoBehaviour
{
    // Velocidad de movimiento aplicada al objeto
    public float moveSpeed = 5f;

    // Variable para almacenar la masa del objeto colisionado
    private float targetMass;

    // Método llamado cuando el personaje colisiona con otro objeto usando un CharacterController
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Obtiene el Rigidbody del objeto colisionado, si tiene uno
        Rigidbody body = hit.collider.attachedRigidbody;

        // Comprueba que el objeto tiene un Rigidbody no cinemático y que el choque es lateral o hacia adelante
        if (body != null && !body.isKinematic && hit.moveDirection.y > -0.3)
        {
            // Almacena la masa del objeto colisionado
            targetMass = body.mass;

            // Calcula la dirección del empuje, solo en el plano horizontal
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            // Aplica velocidad al Rigidbody en la dirección de empuje ajustada por moveSpeed y la masa del objeto
            body.velocity = pushDir * moveSpeed / targetMass;
        }
    }
}