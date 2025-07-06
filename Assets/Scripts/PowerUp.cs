using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpEffect powerUpEffect;
    private PowerUpSpawner spawner;

    public void SetSpawner(PowerUpSpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            powerUpEffect.Apply(collision.gameObject);

            if (spawner != null)
            {
                spawner.OnPowerUpCollected();
            }

            Destroy(gameObject);
        }
    }
}