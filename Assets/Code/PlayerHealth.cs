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
        if (isDead)
            return;

        currentHealth -= damage;

        // Không cho máu xuống dưới 0
        currentHealth = Mathf.Max(currentHealth, 0f);

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // =========================
    // HỒI MÁU
    // =========================
    public bool Heal(float amount)
    {
        // Đã chết thì không hồi
        if (isDead)
            return false;

        // Máu đã đầy thì không nhặt
        if (currentHealth >= maxHealth)
        {
            Debug.Log("Máu đang đầy!");
            return false;
        }

        currentHealth += amount;

        // Không vượt quá máu tối đa
        currentHealth = Mathf.Min(
            currentHealth,
            maxHealth
        );

        Debug.Log("Hồi máu: +" + amount +
                  " | Player HP: " + currentHealth);

        return true;
    }

    private void Die()
    {
        isDead = true;

        Debug.Log("PLAYER DIED");

        PlayerMovement movement =
            GetComponent<PlayerMovement>();

        if (movement != null)
        {
            movement.enabled = false;
        }

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