using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public int maxBullets = 5;

    private int activeBullets = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && activeBullets < maxBullets)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * bulletSpeed;

        activeBullets++;

        // Куршумот автоматски ќе се уништи по 5 секунди
        Destroy(bullet, 5f);
        StartCoroutine(DecreaseBulletAfterDelay(5f));
    }

    IEnumerator DecreaseBulletAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        activeBullets--;
    }
}
