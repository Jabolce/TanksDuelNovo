using UnityEngine;

public class SmartEnemyAI : MonoBehaviour
{
    public float detectionRadius = 10f;
    public float speed = 3f;
    public LayerMask powerUpLayer;
    public LayerMask obstacleLayer;

    public Transform player;

    private Rigidbody2D rb;
    private Transform targetPowerUp;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        targetPowerUp = FindClosestPowerUp();

        float playerDist = Vector2.Distance(transform.position, player.position);
        float powerUpDist = targetPowerUp != null ? Vector2.Distance(transform.position, targetPowerUp.position) : Mathf.Infinity;

        bool playerVisible = CanSee(player.position);
        bool powerUpReachable = targetPowerUp != null && CanSee(targetPowerUp.position);

        if (playerVisible && playerDist < powerUpDist)
        {
            // 👁️ Играчот е поблиску и видлив → застани и ротирај
            StopMovement();
            RotateTowards(player.position);
        }
        else if (powerUpReachable)
        {
            // 🟩 PowerUp достапен → оди кон него
            MoveTowards(targetPowerUp.position);
        }
        else if (!playerVisible && CanSee(player.position))
        {
            // ❌ Не гледа player, но нема PowerUp → оди до player
            MoveTowards(player.position);
        }
        else
        {
            StopMovement();
        }
    }

    void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;
        RotateTowards(target);
    }

    void StopMovement()
    {
        rb.velocity = Vector2.zero;
    }

    void RotateTowards(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    bool CanSee(Vector2 target)
    {
        Vector2 dir = target - (Vector2)transform.position;
        float dist = dir.magnitude;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, dist, obstacleLayer);
        return hit.collider == null;
    }

    Transform FindClosestPowerUp()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, powerUpLayer);
        float minDist = Mathf.Infinity;
        Transform closest = null;

        foreach (Collider2D hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDist && CanSee(hit.transform.position))
            {
                minDist = dist;
                closest = hit.transform;
            }
        }

        return closest;
    }
}