using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float upForce=250f;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
            Debug.Log("Jump");
        }
            
        Debug.Log(context.phase);
    }
}
