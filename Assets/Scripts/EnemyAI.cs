using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour, ISpeedBuff
{
    public Transform player;
    public float moveSpeed = 2f;
    public float obstacleAvoidanceDistance = 1f;
    public LayerMask obstacleLayer; // Set this in Inspector to detect walls

    private float originalSpeed;
    private bool isSpeedModified = false;

    void Start()
    {
        originalSpeed = moveSpeed;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;

        // Obstacle detection using raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleAvoidanceDistance, obstacleLayer);

        if (hit.collider != null)
        {
            // Obstacle ahead — steer left or right
            Vector3 left = Vector3.Cross(direction, Vector3.forward);  // 90 deg to the left
            Vector3 right = Vector3.Cross(direction, Vector3.back);    // 90 deg to the right

            bool leftClear = !Physics2D.Raycast(transform.position, left, obstacleAvoidanceDistance, obstacleLayer);
            bool rightClear = !Physics2D.Raycast(transform.position, right, obstacleAvoidanceDistance, obstacleLayer);

            if (leftClear)
                direction = left;
            else if (rightClear)
                direction = right;
            else
                return; // stuck — don't move
        }

        transform.position += direction * moveSpeed * Time.deltaTime;
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
