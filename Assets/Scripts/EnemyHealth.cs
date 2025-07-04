using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int MaxHealth = 3;
    public int CurrentHealth;

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

    void Die()
    {
        Debug.Log("Player Died!");
        Destroy(gameObject);
        // подоцна: reload scene, game over screen...
    }
}