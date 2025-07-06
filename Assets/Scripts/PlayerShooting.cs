using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour, IShotGun, IMachineGun
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    public GameObject muzzleFlash; // Assign this in the Inspector
    public float flashDuration = 0.05f;

    public int maxBullets = 2;
    private int activeBullets = 0;

    private bool shotgunEnabled = false;

    private bool machineGunEnabled = false;
    public float machineGunFireRate = 0.1f; // 10 bullets per second
    private float currentFireRate;


    // New cooldown variables
    public float fireCooldown = 1f; // 1 second cooldown
    private float fireTimer = 1f;

    void Start()
    {
        currentFireRate = fireCooldown;
        fireTimer = currentFireRate; // allow instant fire
    }

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && activeBullets < maxBullets && fireTimer >= currentFireRate)
        {
            Shoot();
            fireTimer = 0f; // reset cooldown timer on shooting
        }
    }

    void Shoot()
    {
        if (shotgunEnabled)
        {
            FireSpread(4, 60f);  // 4 bullets spread
        }
        else
        {
            FireSingle();
        }
    }

    void FireSingle()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.layer = LayerMask.NameToLayer("PlayerBullet");
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * bulletSpeed;
        activeBullets++;
        Destroy(bullet, 5f);
        StartCoroutine(DecreaseBulletAfterDelay(5f));
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
            bullet.layer = LayerMask.NameToLayer("PlayerBullet");
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = rot * Vector3.up * bulletSpeed;

            activeBullets++;
            Destroy(bullet, 5f);
            StartCoroutine(DecreaseBulletAfterDelay(5f));
        }

        StartCoroutine(ShowMuzzleFlash());

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

    private IEnumerator ShowMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(flashDuration);
            muzzleFlash.SetActive(false); //zipzapzupdsadsa
        }
    }

}
