using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;

    Rigidbody2D rb;
    Vector2 movement;
    float rotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.y = Input.GetAxis("Vertical");
        rotation = -Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + (Vector2)transform.up * movement.y * moveSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation + rotation * rotationSpeed * Time.fixedDeltaTime);
    }
}