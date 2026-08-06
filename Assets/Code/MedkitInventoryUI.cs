using UnityEngine;

public class MedkitInventory : MonoBehaviour
{
    public static MedkitInventory Instance;

    [Header("Medkit")]
    public int medkitCount = 0;
    public float healAmount = 30f;

    [Header("UI")]
    public GameObject medkitSlot;

    void Awake()
    {
        Instance = this;

        if (medkitSlot != null)
            medkitSlot.SetActive(false);
    }

    public void PickupMedkit()
    {
        medkitCount++;

        // Có ít nhất 1 hộp thì hiện slot 3
        if (medkitSlot != null)
            medkitSlot.SetActive(true);

        Debug.Log("Nhặt medkit! Số lượng: " + medkitCount);
    }

    public void UseMedkit()
    {
        if (medkitCount <= 0)
        {
            Debug.Log("Không có hộp cứu thương!");
            return;
        }

        PlayerHealth player =
            FindFirstObjectByType<PlayerHealth>();

        if (player == null)
            return;

        bool healed = player.Heal(healAmount);

        // Máu đầy thì không mất hộp
        if (!healed)
        {
            Debug.Log("Máu đang đầy!");
            return;
        }

        medkitCount--;

        Debug.Log("Đã dùng medkit. Còn: " + medkitCount);

        // Hết medkit thì ẩn slot 3
        if (medkitCount <= 0 && medkitSlot != null)
            medkitSlot.SetActive(false);
    }
}