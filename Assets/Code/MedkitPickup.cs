using UnityEngine;

public class MedkitPickup : MonoBehaviour, IInteractable
{
    [Header("Heal")]
    public float healAmount = 30f;

    public string GetInteractText()
    {
        return "Nhặt hộp cứu thương";
    }

    public void Interact()
    {
        PlayerHealth playerHealth =
            FindFirstObjectByType<PlayerHealth>();

        if (playerHealth == null)
            return;

        bool healed = playerHealth.Heal(healAmount);

        // Chỉ biến mất nếu thực sự hồi được máu
        if (healed)
        {
            Destroy(gameObject);
        }
    }
}