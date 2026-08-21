using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private bool isDead;

    [Header("Damage Sound")]
    public AudioSource audioSource;
    public AudioClip damageSound;

    [Range(0f, 1f)]
    public float damageVolume = 1f;

    [Header("Death")]
    public string mainMenuScene = "MainMenu";
    public float returnToMenuDelay = 2f;

    private void Start()
    {
        currentHealth = maxHealth;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // =========================
    // NHẬN DAMAGE
    // =========================

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        currentHealth = Mathf.Max(
            currentHealth,
            0f
        );

        Debug.Log(
            "Player HP: " +
            currentHealth
        );

        // Âm thanh nhận damage
        if (audioSource != null &&
            damageSound != null)
        {
            audioSource.PlayOneShot(
                damageSound,
                damageVolume
            );
        }

        // Kiểm tra chết
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
        if (isDead)
            return false;

        if (currentHealth >= maxHealth)
        {
            Debug.Log("Máu đang đầy!");
            return false;
        }

        currentHealth += amount;

        currentHealth = Mathf.Min(
            currentHealth,
            maxHealth
        );

        Debug.Log(
            "Hồi máu: +" +
            amount +
            " | Player HP: " +
            currentHealth
        );

        return true;
    }

    // =========================
    // PLAYER DIE
    // =========================

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("PLAYER DIED");

        // Tắt di chuyển
        PlayerMovement movement =
            GetComponent<PlayerMovement>();

        if (movement != null)
        {
            movement.enabled = false;
        }

        // Chờ một chút rồi về Main Menu
        Invoke(
            nameof(ReturnToMainMenu),
            returnToMenuDelay
        );
    }

    // =========================
    // RETURN MAIN MENU
    // =========================

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuScene);
    }

    // =========================
    // GET HEALTH
    // =========================

    public float GetHealth()
    {
        return currentHealth;
    }
}