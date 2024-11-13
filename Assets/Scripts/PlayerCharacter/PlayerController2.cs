using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Velocidad de movimiento del jugador

    public CharacterController player; // Componente que maneja las colisiones y el movimiento del personaje
    private Vector3 movePlayer; // Dirección en la que se debe mover el jugador

    // Variables para controlar la cámara
    public Camera mainCamera; // Cámara principal
    private Vector3 camForward; // Dirección hacia adelante de la cámara
    private Vector3 camRight; // Dirección hacia la derecha de la cámara

    private PlayerInput playerInput; // Entrada del jugador
    private Vector3 input; // Vector que almacena la entrada del jugador

    //Gravedad
    public float gravedad = 9.8f;
    public float fallVelocity;


    //Salto
    public float jumpVelocity;

    //Controlar que tenga caida en pendientes superiores a las que pueda subir
    public bool isOnSlope = false;
    public Vector3 hitNormal;
    public float slideVelocity;
    public float slopeForceDown;


    // Inicialización de variables y componentes
    void Start()
    {
        playerInput = GetComponent<PlayerInput>(); // Obtener el componente de entrada
        player = GetComponent<CharacterController>(); // Obtener el controlador de personaje
        fallVelocity = 0f; // para que no salte al iniciar juego
    }

    // Método de actualización llamado en cada frame
    void Update()
    {
        // Llamar a las funciones de movimiento y gravedad antes de llamar a Move
        camDirection();
        HandleMovement();
        SetGravity();

        // Llamar a Move solo al final
        player.Move(movePlayer * Time.deltaTime);
    }

// Método para calcular el movimiento del jugador basado en la cámara
    private void HandleMovement()
    {
        Vector2 inputV2 = playerInput.actions["Move"].ReadValue<Vector2>();
        input = new Vector3(inputV2.x, 0, inputV2.y);
        input = Vector3.ClampMagnitude(input, 1);

        movePlayer = input.x * camRight + input.z * camForward;
        movePlayer *= speed;
        player.transform.LookAt(player.transform.position + movePlayer);
    }

// Ajuste para la gravedad y el salto
    private void SetGravity()
    {
        if (player.isGrounded)
        {
            // Reiniciar la velocidad de caída al tocar el suelo
            fallVelocity = -gravedad * Time.deltaTime;

            // Detectar si se presiona el salto
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fallVelocity = jumpVelocity;
            }
        }
        else
        {
            // Aplicar gravedad en el aire
            fallVelocity -= gravedad * Time.deltaTime;
        }

        movePlayer.y = fallVelocity;

        SlideDown(); // controla pendientes
    }

    // Método para obtener la dirección de la cámara
    private void camDirection()
    {
        camForward = mainCamera.transform.forward; // Dirección hacia adelante de la cámara
        camRight = mainCamera.transform.right; // Dirección hacia la derecha de la cámara

        camForward.y = 0; // Ignorar el componente y para que la cámara no afecte la altura
        camRight.y = 0;

        camForward = camForward.normalized; // Normalizar el vector para mantener la misma dirección
        camRight = camRight.normalized;
    }


    public void SlideDown()
    {
        /*
         *

            Vector3.Angle calcula el ángulo entre dos vectores en grados.
            Vector3.up representa un vector apuntando hacia arriba (es decir, (0, 1, 0)),
            lo cual indica la dirección "vertical".
            hitNormal es la normal de la superficie en el punto de colisión.
            Si el jugador está sobre una superficie inclinada, hitNormal estará en un ángulo diferente a Vector3.up.
            Este cálculo devuelve el ángulo en grados entre la dirección "arriba" (Vector3.up) y la normal de la superficie de colisión (hitNormal). Un ángulo mayor significa una pendiente más pronunciada.
         *
         */
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;
        if (isOnSlope)
        {
            // mas rapido si la pendiente es mayor
            movePlayer.x += (1 - hitNormal.y) * hitNormal.x * slideVelocity;
            movePlayer.z +=  (1 - hitNormal.y) *hitNormal.z * slideVelocity;

            //para que no de saltos al caer, debe ser negativo
            movePlayer.y += slopeForceDown;
        }


        Debug.Log(Vector3.Angle(Vector3.up, hitNormal));
    }

    // es un método en Unity que se llama automáticamente cuando un CharacterController choca con otro objeto mientras se mueve.
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //hit.normal en Unity representa la normal de la superficie en el punto exacto donde ocurre la colisión
        hitNormal = hit.normal;
    }
}