using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour, IShotGun, IMachineGun
{
    public Transform player;
    public Transform turret;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 8f;
    public float fireRate = 1f;
    public LayerMask obstacleLayer;

    public GameObject muzzleFlash;
    public float flashDuration = 0.05f;

    private float fireTimer;
    private bool shotgunEnabled = false;
    private bool machineGunEnabled = false;

    public float machineGunFireRate = 0.1f;
    private float currentFireRate;
    public float normalFireRate = 1f;

    void Start()
    {
        currentFireRate = normalFireRate;
    }

    void Update()
    {
        if (player == null) return;

        fireTimer += Time.deltaTime;

        if (CanSeePlayer())
        {
            RotateTowardsPlayer();

            if (IsFacingPlayer() && fireTimer >= currentFireRate)
            {
                Shoot();
                fireTimer = 0f;
            }
        }
    }

    void RotateTowardsPlayer()
    {
        Vector2 direction = player.position - turret.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        turret.rotation = Quaternion.RotateTowards(
            turret.rotation,
            Quaternion.Euler(0, 0, angle),
            200f * Time.deltaTime
        );
    }

    bool IsFacingPlayer()
    {
        Vector2 toPlayer = (player.position - turret.position).normalized;
        Vector2 facing = turret.up;
        return Vector2.Angle(facing, toPlayer) < 15f;
    }

    bool CanSeePlayer()
    {
        Vector2 dir = player.position - firePoint.position;
        float dist = dir.magnitude;
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, dir.normalized, dist, obstacleLayer);

        return hit.collider == null || hit.collider.transform == player;
    }

    void Shoot()
    {
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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * bulletSpeed;
        StartCoroutine(ShowMuzzleFlash());
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
            rb.velocity = rot * Vector2.up * bulletSpeed;
        }

        StartCoroutine(ShowMuzzleFlash());
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
        currentFireRate = normalFireRate;
    }
}
