using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour, ISpeedBuff
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;
    private TrailRenderer trail;


    private float originalSpeed;
    private bool isSpeedModified = false;

    Rigidbody2D rb;
    Vector2 movement;
    float rotation;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        if (trail != null)
            trail.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = moveSpeed; 
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

    public void ModifySpeed(float amount, float duration)
    {
        if (isSpeedModified) return;

        moveSpeed += amount;
        isSpeedModified = true;

        // Enable trail effect while boosted
        if (trail != null)
            trail.enabled = true;

        StartCoroutine(ResetSpeedAfter(duration));
    }

    private IEnumerator ResetSpeedAfter(float duration)
    {
        yield return new WaitForSeconds(duration);

        moveSpeed = originalSpeed;
        isSpeedModified = false;

        // Disable trail effect after boost ends
        if (trail != null)
            trail.enabled = false;
    }
}