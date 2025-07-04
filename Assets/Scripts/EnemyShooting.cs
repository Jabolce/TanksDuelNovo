using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour, IShotGun, IMachineGun
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 8f;
    public float fireRate = 2f;


    private bool shotgunEnabled = false;

    private float fireTimer = 1f;
    public float fireCooldown = 1f;

    private bool machineGunEnabled = false;
    public float machineGunFireRate = 0.1f;
    private float currentFireRate;

    public Transform player;
    public LayerMask obstacleLayer;

    void Start()
    {
        currentFireRate = fireCooldown;
    }


    void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= currentFireRate)
        {
            if (CanSeePlayer() || CanHitPlayerWithRicochet())
            {
                Shoot();
                fireTimer = 0f;
            }
        }
    }

    void Shoot()
    {
        if (shotgunEnabled)
        {
            FireSpread(4, 60f); // 4 bullets, 60 degrees spread
        }
        else
        {
            FireSingle();
        }
    }

    bool CanHitPlayerWithRicochet()
    {
        if (player == null) return false;

        Vector2 start = firePoint.position;
        Vector2 direction = firePoint.up;
        float maxDistance = 20f; // total range to test

        // First raycast — see if we hit a wall
        RaycastHit2D firstHit = Physics2D.Raycast(start, direction, maxDistance, obstacleLayer);
        if (firstHit.collider != null)
        {
            // Reflect the direction based on surface normal
            Vector2 reflectedDir = Vector2.Reflect(direction, firstHit.normal);

            // Second raycast from bounce point
            RaycastHit2D secondHit = Physics2D.Raycast(firstHit.point + reflectedDir * 0.1f, reflectedDir, maxDistance, obstacleLayer | LayerMask.GetMask("Player"));

            if (secondHit.collider != null && secondHit.collider.CompareTag("Player"))
            {
                return true; // player can be hit by ricochet
            }
        }

        return false;
    }

    void FireSingle()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * bulletSpeed;
    }

    void FireSpread(int bulletCount, float totalAngle)
    {
        float angleStep = totalAngle / (bulletCount - 1);
        float startAngle = -totalAngle / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Quaternion rot = firePoint.rotation * Quaternion.Euler(0, 0, angle);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rot);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = rot * Vector3.up * bulletSpeed;
        }
    }

    public void EnableShotgun(float duration)
    {
        if (!shotgunEnabled)
        {
            shotgunEnabled = true;
            StartCoroutine(DisableShotgunAfter(duration));
        }
    }

    IEnumerator DisableShotgunAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        shotgunEnabled = false;
    }

    public void EnableMachineGun(float duration)
    {
        if (!machineGunEnabled)
        {
            machineGunEnabled = true;
            currentFireRate = machineGunFireRate;
            StartCoroutine(DisableMachineGunAfter(duration));
        }
    }

    private IEnumerator DisableMachineGunAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        machineGunEnabled = false;
        currentFireRate = fireCooldown;
    }

    public bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 direction = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, direction.magnitude, obstacleLayer);

        // If the ray hits nothing, OR hits the player directly
        if (hit.collider == null) return true; // no obstacles
        if (hit.collider.transform == player) return true;

        return false; // blocked by wall
    }
}
