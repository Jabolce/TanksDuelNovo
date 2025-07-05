using UnityEngine;

public class EnemyPowerUpSeeker : MonoBehaviour
{
    public float detectionRadius = 20f;
    public LayerMask powerUpLayer;
    public float speed = 3f;
    private Transform targetPowerUp;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        FindClosestPowerUp();

        if (targetPowerUp != null)
        {
            Vector2 direction = (targetPowerUp.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void FindClosestPowerUp()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, powerUpLayer);
        Debug.Log("PowerUps found: " + hits.Length); // додади ова

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

        targetPowerUp = closest;
    }

}