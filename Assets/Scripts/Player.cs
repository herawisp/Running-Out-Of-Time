using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;

    private Rigidbody rb;
    private Animator animator;
    private Vector2 movementInput;

    private int lastLookDirection = 1;
        
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (movementInput.magnitude == 0)
        {
            animator.SetBool("isWalking", false);
        } else
        {
            animator.SetBool("isWalking", true);
            lastLookDirection = (movementInput.x > 0)? -1: 1;
        }
        transform.localScale = new Vector3(lastLookDirection, 1, 1);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(movementInput.x * moveSpeed, 0f, movementInput.y * moveSpeed);
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }
}
