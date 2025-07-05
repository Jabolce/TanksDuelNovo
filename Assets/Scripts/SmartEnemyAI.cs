using UnityEngine;

public class SmartEnemyAI : MonoBehaviour
{
    public float detectionRadius = 15f;
    public LayerMask powerUpLayer;
    public Transform player;
    public float speed = 3f;

    private Rigidbody2D rb;
    private Transform currentTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Побарај најблиску PowerUp
        Transform powerUp = FindClosestPowerUp();

        // 2. Ако има PowerUp → тргни кон него, инаку кон играчот
        if (powerUp != null)
        {
            currentTarget = powerUp;
        }
        else
        {
            currentTarget = player;
        }

        // 3. Движење кон целта
        Vector2 direction = (currentTarget.position - transform.position).normalized;
        rb.velocity = direction * speed;

        // 4. Ротирање кон целта
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    Transform FindClosestPowerUp()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, powerUpLayer);
        float minDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (Collider2D hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = hit.transform;
            }
        }

        return closest;
    }
}