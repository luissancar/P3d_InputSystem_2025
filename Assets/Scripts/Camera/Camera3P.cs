using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase para controlar una cámara de tercera persona que sigue al jugador y permite rotación controlada por el ratón


//En cinemachine
//cambiar Orbits -> BottomRig =0.05
// cambiar Lents-> Vertical FOV 120 distancia


public class Camera3P : MonoBehaviour
{
    // Desplazamiento inicial de la cámara respecto al jugador
    public Vector3 offset;
    
    // Transform del objeto jugador (la cámara seguirá a este objetivo)
    private Transform target;
    
    // Valor de interpolación para suavizar el movimiento de la cámara
    [Range(0, 1)] public float lerpValue;
    
    // Sensibilidad del movimiento de la cámara con el ratón
    [SerializeField] private float sensibilidad;

    // Variables comentadas para limitar el ángulo vertical (no utilizadas actualmente)
    // public float verticalMinLimit = -30f;
    // public float verticalMaxLimit = 60f;

    // Se ejecuta al inicio del juego
    void Start()
    {
        // Encuentra el objeto "Player" en la escena y almacena su Transform como el objetivo
        target = GameObject.Find("Player").transform;
    }

    // Se ejecuta después de Update en cada frame, ideal para la cámara
    private void LateUpdate()
    {
        // Aplica la rotación horizontal de la cámara en función del movimiento del ratón en el eje X
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * sensibilidad, Vector3.up) * offset;

        // Código comentado para limitar la rotación vertical
        // Calcula y limita el ángulo vertical en el eje Y del ratón
        // float angleY = Mathf.Clamp(Input.GetAxis("Mouse Y") * sensibilidad, verticalMinLimit, verticalMaxLimit);
        // offset = Quaternion.AngleAxis(angleY, Vector3.right) * offset;

        // Actualiza la posición de la cámara interpolando entre su posición actual y la posición objetivo
        transform.position = Vector3.Lerp(transform.position, target.position + offset, lerpValue);

        // Hace que la cámara mire hacia el objetivo (jugador)
        transform.LookAt(target);
    }
}
/*

offset: Define la posición inicial de la cámara respecto al jugador.
lerpValue: Permite ajustar la suavidad del movimiento de la cámara.
sensibilidad: Controla la sensibilidad de la rotación de la cámara en respuesta al movimiento del ratón.
target: Se asigna al jugador para que la cámara lo siga.
Rotación: La rotación horizontal depende del movimiento del ratón en el eje X.
Limitación vertical (comentada): Aunque las variables verticalMinLimit y verticalMaxLimit están definidas, están comentadas y no limitan la rotación vertical actualmente.
 
 */
