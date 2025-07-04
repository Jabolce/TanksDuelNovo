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

        // ROTATE to face player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            180f * Time.deltaTime // adjust rotation speed here
        );

        // Check for obstacles
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleAvoidanceDistance, obstacleLayer);

        if (hit.collider != null)
        {
            // Try to steer around
            Vector3 left = Vector3.Cross(direction, Vector3.forward);
            Vector3 right = Vector3.Cross(direction, Vector3.back);

            bool leftClear = !Physics2D.Raycast(transform.position, left, obstacleAvoidanceDistance, obstacleLayer);
            bool rightClear = !Physics2D.Raycast(transform.position, right, obstacleAvoidanceDistance, obstacleLayer);

            if (leftClear)
                direction = left;
            else if (rightClear)
                direction = right;
            else
                return; // stuck
        }

        // Move forward in the direction we're currently facing
        transform.position += transform.up * moveSpeed * Time.deltaTime;
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
