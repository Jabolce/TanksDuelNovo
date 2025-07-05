using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour, ISpeedBuff
{
    public Transform player;
    public float moveSpeed = 2f;
    public float rotationSpeed = 180f;
    public float obstacleAvoidanceDistance = 1f;
    public LayerMask obstacleLayer;

    private Rigidbody2D rb;
    private float originalSpeed;
    private bool isSpeedModified = false;
    private Vector3 targetDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = moveSpeed;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 toPlayer = (player.position - transform.position).normalized;

        // Obstacle avoidance check
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, obstacleAvoidanceDistance, obstacleLayer);

        if (hit.collider != null)
        {
            float bestAngle = 0f;
            float bestDistance = 0f;

            for (int i = -90; i <= 90; i += 15)
            {
                Vector2 dir = Quaternion.Euler(0, 0, i) * transform.up;
                RaycastHit2D check = Physics2D.Raycast(transform.position, dir, obstacleAvoidanceDistance, obstacleLayer);
                float dist = check.collider == null ? obstacleAvoidanceDistance : check.distance;

                if (dist > bestDistance)
                {
                    bestDistance = dist;
                    bestAngle = i;
                }
            }

            targetDirection = Quaternion.Euler(0, 0, bestAngle) * transform.up;
        }
        else
        {
            targetDirection = toPlayer;
        }

        // ROTATE smoothly
        float currentAngle = rb.rotation;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(newAngle);

        // MOVE forward
        rb.MovePosition(rb.position + (Vector2)transform.up * moveSpeed * Time.fixedDeltaTime);
    }

    public void ModifySpeed(float amount, float duration)
    {
        if (isSpeedModified) return;

        moveSpeed += amount;
        isSpeedModified = true;
        StartCoroutine(ResetSpeed(duration));
    }

    private IEnumerator ResetSpeed(float duration)
    {
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
        isSpeedModified = false;
    }
}
