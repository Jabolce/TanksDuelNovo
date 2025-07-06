using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealable
{
    public int MaxHealth = 3;
    public int CurrentHealth;
    public GameObject explosionPrefab;
    public GameObject gameOverPanel;

    public HealthBar enemyHealthBar;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        enemyHealthBar.SetMaxHealth(MaxHealth);
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        enemyHealthBar.SetHealth(CurrentHealth);
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;

        enemyHealthBar.SetHealth(CurrentHealth);
    }


    void Die()
    {
        Debug.Log("Player Died!");
        
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Destroy(gameObject);
        // подоцна: reload scene, game over screen...
    }
}