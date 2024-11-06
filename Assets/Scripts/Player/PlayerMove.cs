using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//Video 23 minutos
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float upForce=10f;
    [SerializeField] private float speed=10f;
    public float groundCheckDistance = 1f; // Distancia para verificar si está en el suelo
    public LayerMask groundLayer;          // Capa del suelo para verificar colisiones con Raycast

    private Rigidbody rb;
    
    
    private PlayerInput playerInput;
    private Vector2 input;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    
    void Update()
    {
        input=playerInput.actions["Move"].ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(input.x,0f,input.y)*speed);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            if (context.performed)
            {
                rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
                Debug.Log("Jump");
            }
        }
            
        Debug.Log(context.phase);
    }
    
    
    /*
     * Para crear una capa específica para el suelo en Unity y seleccionarla en el Inspector, sigue estos pasos:

Crear la capa de suelo:

En la esquina superior derecha de la ventana de Unity, ve a "Layers" y haz clic en el menú desplegable.
Selecciona "Edit Layers...".
En la sección "User Layers", elige un espacio vacío (por ejemplo, Layer 8) y escribe el nombre de la nueva capa, como "Ground" o "Suelo".
Una vez que la hayas nombrado, la capa estará disponible para asignarse a objetos.
Asignar la capa al suelo:

Selecciona el objeto del suelo en la jerarquía (por ejemplo, el terreno o cualquier objeto que desees usar como superficie para que el personaje pueda saltar).
En el Inspector, busca la opción "Layer" en la parte superior de la ventana.
Haz clic en el menú desplegable y selecciona la capa que acabas de crear ("Ground" o "Suelo").
Cuando Unity pregunte si deseas asignar esta capa a todos los objetos hijos, selecciona "Yes" si quieres que todos los objetos anidados en el suelo tengan la misma capa.
Configurar el script para detectar la capa de suelo:

En tu script, verás la variable groundLayer. Esta variable de tipo LayerMask permite que selecciones la capa de suelo desde el Inspector sin tener que escribir código adicional.
Selecciona el objeto que tiene el script (por ejemplo, el jugador o el objeto que salta).
En el Inspector, busca la sección del script y encuentra la variable groundLayer.
Selecciona el nuevo layer ("Ground" o "Suelo") desde el menú desplegable de groundLayer.

al suelo asignarle el layer 
     * */
     
    public   bool IsGrounded()
    {
      
        // Lanza un rayo desde la posición del objeto hacia abajo
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}
