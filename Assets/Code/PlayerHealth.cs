using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  public float maxHealth = 100f;
private float currentHealth;

    
    private bool isDead;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        Debug.Log("PLAYER DIED");

        // Tắt di chuyển
        GetComponent<PlayerMovement>().enabled = false;

        // TODO:
        // Hiện Game Over
        // Respawn
        // Animation chết
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}