using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpEffect powerUpEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("PowerUp hit: " + collision.name);  // Додади ова

        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            powerUpEffect.Apply(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
