using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 1f;
    public bool isDead = false;

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        
        health -= amount;
        
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        
        // Notify game manager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerDied();
        }
        
        // Disable player
        gameObject.SetActive(false);
    }
}
