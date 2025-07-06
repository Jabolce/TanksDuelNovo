using UnityEngine;
using Pathfinding;
using System.Collections;

public class SmartEnemyAI : MonoBehaviour, ISpeedBuff
{
    public Transform player;
    public Transform[] powerUps;
    public float updateRate = 0.5f;
    public LayerMask obstacleMask;

    private Seeker seeker;
    private AIPath aiPath;
    private Path path;
    private int currentWaypoint = 0;
    private Transform target;

    private float baseSpeed;
    private bool isSpeedModified = false;
    private TrailRenderer trail;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();

        if (aiPath == null)
        {
            Debug.LogError("AIPath component not found on enemy!");
            return;
        }

        baseSpeed = aiPath.maxSpeed;

        trail = GetComponent<TrailRenderer>();
        if (trail != null)
            trail.enabled = false;

        InvokeRepeating(nameof(UpdatePath), 0f, updateRate);
    }

    void UpdatePath()
    {
        target = ChooseTarget();

        if (seeker.IsDone() && target != null)
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete); // zlip zlorp
        }
    }

    Transform ChooseTarget()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        bool canSeePlayer = !Physics2D.Raycast(transform.position, player.position - transform.position,
                                               distanceToPlayer, obstacleMask);

        foreach (Transform p in powerUps)
        {
            float d = Vector2.Distance(transform.position, p.position);
            if (d < distanceToPlayer)
                return p;
        }

        return canSeePlayer ? player : FindClosestPowerUp();
    }

    Transform FindClosestPowerUp()
    {
        Transform closest = null;
        float closestDist = Mathf.Infinity;

        foreach (Transform p in powerUps)
        {
            float d = Vector2.Distance(transform.position, p.position);
            if (d < closestDist)
            {
                closestDist = d;
                closest = p;
            }
        }

        return closest != null ? closest : player;
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // ISpeedBuff implementation
    public void ModifySpeed(float amount, float duration)
    {
        if (isSpeedModified || aiPath == null) return;

        aiPath.maxSpeed += amount;
        isSpeedModified = true;

        if (trail != null)
            trail.enabled = true;

        StartCoroutine(ResetSpeedAfter(duration));
    }

    private IEnumerator ResetSpeedAfter(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (aiPath != null)
            aiPath.maxSpeed = baseSpeed;

        isSpeedModified = false;

        if (trail != null)
            trail.enabled = false;
    }
}
