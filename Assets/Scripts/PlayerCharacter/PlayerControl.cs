using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Clase que controla el movimiento y acciones del jugador
public class PlayerControl : MonoBehaviour
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
    public bool isGro;

    // Inicialización de variables y componentes
    void Start()
    {
        playerInput = GetComponent<PlayerInput>(); // Obtener el componente de entrada
        player = GetComponent<CharacterController>(); // Obtener el controlador de personaje
    }

    // Método de actualización llamado en cada frame
    void Update()
    {
        isGro = player.isGrounded;
        // Leer el valor de entrada del jugador (movimiento en el eje x y z)
        Vector2 inputV2 = playerInput.actions["Move"].ReadValue<Vector2>();
        input = new Vector3(inputV2.x, 0, inputV2.y); // Crear un vector con la entrada en x y z
        input = Vector3.ClampMagnitude(input, 1); // Limitar la magnitud del vector a 1

        // Calcular la dirección de la cámara y mover al jugador en esa dirección
        camDirection();
        movePlayer =
            input.x * camRight + input.z * camForward; // Calcular el movimiento del jugador en base a la cámara
        movePlayer = movePlayer * speed;
        player.transform.LookAt(player.transform.position +
                                movePlayer); // Hacer que el jugador mire hacia la dirección de movimiento

        //Gravedad
        SetGravity();
        //Salto
      //  Jump2();
        // Mover al jugador usando el CharacterController
        player.Move(movePlayer * Time.deltaTime); // Mover al jugador en la dirección y velocidad establecidas
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


    public void Jump2()
    {
        Debug.Log("jump2  ");
        if (player.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            fallVelocity = jumpVelocity;
            movePlayer.y = fallVelocity;
        }
    }

    // Método para el salto
    public void Jump(InputAction.CallbackContext context)
    {
        // Debug.Log("Jump1");
         if (player.isGrounded) // Verificar si el jugador está en el suelo
         {
             Debug.Log("Jump2");
             if (context.performed) // Si la acción de salto se realizó
             {
                 Debug.Log("Jump3");
                 fallVelocity = jumpVelocity;
                 movePlayer.y = fallVelocity;
             }
         }

         Debug.Log(context.phase); // Imprimir el estado de la acción en la consola*/
    }


    private void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity = -gravedad * Time.deltaTime;
        }
        else
        {
            fallVelocity -= gravedad * Time.deltaTime;
        }

        movePlayer.y = fallVelocity;
    }
}