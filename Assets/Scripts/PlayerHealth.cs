using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealable
{
    public int MaxHealth = 3;
    public int CurrentHealth;

    public HealthBar healthBar;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        healthBar.SetHealth(CurrentHealth);
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

        healthBar.SetHealth(CurrentHealth);
    }


    void Die()
    {
        Debug.Log("Player Died!");
        Destroy(gameObject);
        // подоцна: reload scene, game over screen...
    }
}