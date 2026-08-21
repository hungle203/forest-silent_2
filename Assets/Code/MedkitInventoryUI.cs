using UnityEngine;

public class MedkitInventory : MonoBehaviour
{
    public static MedkitInventory Instance;

    [Header("Medkit")]
    public int medkitCount = 0;
    public float healAmount = 30f;

    [Header("UI")]
    public GameObject medkitSlot;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip healSound;

    [Range(0f, 1f)]
    public float healVolume = 1f;

    void Awake()
    {
        Instance = this;

        if (medkitSlot != null)
            medkitSlot.SetActive(false);

        // Nếu chưa kéo AudioSource vào Inspector
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // =====================================================
    // PICKUP MEDKIT
    // =====================================================

    public void PickupMedkit()
    {
        medkitCount++;

        // Có ít nhất 1 hộp thì hiện slot 3
        if (medkitSlot != null)
            medkitSlot.SetActive(true);

        Debug.Log(
            "Nhặt medkit! Số lượng: " +
            medkitCount
        );
    }

    // =====================================================
    // USE MEDKIT
    // =====================================================

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

        bool healed =
            player.Heal(healAmount);

        // Máu đầy thì không mất hộp
        if (!healed)
        {
            Debug.Log("Máu đang đầy!");
            return;
        }

        // =========================
        // PHÁT ÂM THANH HỒI MÁU
        // =========================

        if (audioSource != null &&
            healSound != null)
        {
            audioSource.PlayOneShot(
                healSound,
                healVolume
            );
        }

        // =========================
        // TRỪ MEDKIT
        // =========================

        medkitCount--;

        Debug.Log(
            "Đã dùng medkit. Còn: " +
            medkitCount
        );

        // Hết medkit thì ẩn slot 3
        if (medkitCount <= 0 &&
            medkitSlot != null)
        {
            medkitSlot.SetActive(false);
        }
    }
}