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

    void Start()
    {
        currentFireRate = fireCooldown;
    }


    void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= currentFireRate)
        {
            Shoot();
            fireTimer = 0f;
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
}
