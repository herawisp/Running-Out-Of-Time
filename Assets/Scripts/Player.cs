using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;

    private Rigidbody rb;
    private Vector2 movementInput;
        
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(movementInput.x * moveSpeed, 0f, movementInput.y * moveSpeed);
    }

    public void onMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }
}
