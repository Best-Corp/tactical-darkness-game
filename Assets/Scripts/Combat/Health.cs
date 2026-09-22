using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public int maxHealth = 1;
    public int currentHealth;

    public event Action<Health> Died;

    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Died?.Invoke(this);

        gameObject.SetActive(false);
    }
}
