using System.Collections;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject powerUpPrefab;
    public Transform[] spawnPoints;
    public float respawnDelay = 5f;
    public float lifetime = 10f;

    private GameObject currentPowerUp;

    void Start()
    {
        SpawnPowerUp();
    }

    void SpawnPowerUp()
    {
        if (currentPowerUp != null)
        {
            Destroy(currentPowerUp);
        }

        Transform spawnPoint = GetValidSpawnPoint();

        if (spawnPoint != null)
        {
            currentPowerUp = Instantiate(powerUpPrefab, spawnPoint.position, Quaternion.identity);

            PowerUp powerUpScript = currentPowerUp.GetComponent<PowerUp>();
            if (powerUpScript != null)
            {
                powerUpScript.SetSpawner(this);
            }

            StartCoroutine(AutoDespawn(currentPowerUp));
        }
    }

    IEnumerator AutoDespawn(GameObject spawnedPowerUp)
    {
        yield return new WaitForSeconds(lifetime);

        // Проверка дали некој друг го уништил
        if (spawnedPowerUp != null && spawnedPowerUp == currentPowerUp)
        {
            Destroy(spawnedPowerUp);
            StartCoroutine(RespawnAfterDelay());
        }
    }

    public void OnPowerUpCollected()
    {
        if (currentPowerUp != null)
        {
            Destroy(currentPowerUp);
        }

        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnPowerUp();
    }

    Transform GetValidSpawnPoint()
    {
        int tries = 20;
        for (int i = 0; i < tries; i++)
        {
            Transform p = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Collider2D hit = Physics2D.OverlapCircle(p.position, 0.2f, LayerMask.GetMask("Wall"));

            if (hit == null)
                return p;
        }

        return null;
    }
}
