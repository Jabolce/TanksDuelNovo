using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour, IShotGun, IMachineGun
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 8f;
    public float fireRate = 2f;

    private bool shotgunEnabled = false;
    private bool machineGunEnabled = false;

    private float fireTimer = 1f;
    public float fireCooldown = 1f;
    public float machineGunFireRate = 0.1f;
    private float currentFireRate;

    public Transform player;
    public LayerMask obstacleLayer;

    public Transform turretTransform; // 👈 ново поле за ротирање

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
        // Ротирај турет кон играчот пред пукање
        if (player != null && turretTransform != null)
        {
            Vector2 lookDir = player.position - turretTransform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            turretTransform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (shotgunEnabled)
        {
            FireSpread(4, 60f);
        }
        else
        {
            FireSingle();
        }
    }
    void FireSingle()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.layer = LayerMask.NameToLayer("EnemyBullet");
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Vector2 direction = (player.position - firePoint.position).normalized;
        rb.velocity = direction * bulletSpeed;
    }

    void FireSpread(int bulletCount, float totalAngle)
    {
        Vector2 baseDirection = (player.position - firePoint.position).normalized;
        float angleToPlayer = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;

        float angleStep = totalAngle / (bulletCount - 1);
        float startAngle = angleToPlayer - totalAngle / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            bullet.layer = LayerMask.NameToLayer("EnemyBullet");
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = rotation * Vector2.up * bulletSpeed;
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

    IEnumerator DisableMachineGunAfter(float duration)
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

        if (hit.collider == null) return true;
        if (hit.collider.transform == player) return true;

        return false;
    }

    bool CanHitPlayerWithRicochet()
    {
        if (player == null) return false;

        Vector2 start = firePoint.position;
        Vector2 direction = firePoint.up;
        float maxDistance = 20f;

        RaycastHit2D firstHit = Physics2D.Raycast(start, direction, maxDistance, obstacleLayer);
        if (firstHit.collider != null)
        {
            Vector2 reflectedDir = Vector2.Reflect(direction, firstHit.normal);

            RaycastHit2D secondHit = Physics2D.Raycast(firstHit.point + reflectedDir * 0.1f, reflectedDir, maxDistance, obstacleLayer | LayerMask.GetMask("Player"));

            if (secondHit.collider != null && secondHit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }
}
