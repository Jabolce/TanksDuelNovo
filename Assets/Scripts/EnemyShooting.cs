using System.Collections;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 8f;
    public float fireRate = 2f;
    public GameObject muzzleFlash;
    public float flashDuration = 0.05f;

    public LayerMask obstacleLayer;

    private float fireTimer = 0f;

    private bool shotgunEnabled = false;
    private bool machineGunEnabled = false;
    public float machineGunFireRate = 0.1f;
    public float fireCooldown = 1f;
    private float currentFireRate;

    void Start()
    {
        currentFireRate = fireRate;
    }

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (player != null && CanSeePlayer())
        {
            if (IsFacingPlayer())
            {
                if (fireTimer >= currentFireRate)
                {
                    if (shotgunEnabled)
                    {
                        FireSpread(5, 45f);
                    }
                    else
                    {
                        Shoot();
                    }

                    fireTimer = 0f;
                }
            }
        }
    }

    bool IsFacingPlayer()
    {
        Vector2 toPlayer = (player.position - transform.position).normalized;
        Vector2 facing = transform.up;
        float angle = Vector2.Angle(facing, toPlayer);
        return angle < 10f;
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 dir = player.position - transform.position;
        float dist = dir.magnitude;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, dist, obstacleLayer);

        return hit.collider == null || hit.collider.transform == player;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 direction = (player.position - firePoint.position).normalized;
        rb.velocity = direction * bulletSpeed;
        StartCoroutine(ShowMuzzleFlash());
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

        StartCoroutine(ShowMuzzleFlash());
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

    private IEnumerator ShowMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(flashDuration);
            muzzleFlash.SetActive(false);
        }
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
