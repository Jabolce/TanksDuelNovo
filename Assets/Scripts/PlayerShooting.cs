using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour, IShotGun
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    public int maxBullets = 1;
    private int activeBullets = 0;

    private bool shotgunEnabled = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && activeBullets < maxBullets)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (shotgunEnabled)
        {
            FireSpread(4, 60f);  // 4 bullets, 30 degrees spread
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

        activeBullets++;
        Destroy(bullet, 5f);
        StartCoroutine(DecreaseBulletAfterDelay(5f));
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

            activeBullets++;
            Destroy(bullet, 5f);
            StartCoroutine(DecreaseBulletAfterDelay(5f));
        }
    }

    IEnumerator DecreaseBulletAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        activeBullets--;
    }

    // Method to enable shotgun mode from outside (like power-ups)
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
}
