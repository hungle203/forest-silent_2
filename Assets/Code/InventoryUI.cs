using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [Header("Colors")]
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(1f, 1f, 1f, 0.3f);

    [Header("Slots")]
    public GameObject katanaSlot;
    public GameObject gunSlot;
    public GameObject medkitSlot;

    [Header("Slot Images")]
    public Image katanaImage;
    public Image gunImage;
    public Image medkitImage;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (katanaSlot != null)
            katanaSlot.SetActive(false);

        if (gunSlot != null)
            gunSlot.SetActive(false);

        if (medkitSlot != null)
            medkitSlot.SetActive(false);
    }

    // =========================
    // HIỆN SLOT
    // =========================

    public void ShowKatana()
    {
        if (katanaSlot != null)
            katanaSlot.SetActive(true);
    }

    public void ShowGun()
    {
        if (gunSlot != null)
            gunSlot.SetActive(true);
    }

    public void ShowMedkit()
    {
        if (medkitSlot != null)
            medkitSlot.SetActive(true);
    }

    // =========================
    // CHỌN SÚNG
    // =========================

    public void SelectGun()
    {
        SetColor(gunImage, activeColor);
        SetColor(katanaImage, inactiveColor);
        SetColor(medkitImage, inactiveColor);
    }

    // =========================
    // CHỌN KATANA
    // =========================

    public void SelectKatana()
    {
        SetColor(katanaImage, activeColor);
        SetColor(gunImage, inactiveColor);
        SetColor(medkitImage, inactiveColor);
    }

    // =========================
    // CHỌN MEDKIT
    // =========================

    public void SelectMedkit()
    {
        SetColor(medkitImage, activeColor);
        SetColor(gunImage, inactiveColor);
        SetColor(katanaImage, inactiveColor);
    }

    // =========================
    // TAY KHÔNG
    // =========================

    public void SelectNone()
    {
        SetColor(gunImage, inactiveColor);
        SetColor(katanaImage, inactiveColor);
        SetColor(medkitImage, inactiveColor);
    }

    void SetColor(Image image, Color color)
    {
        if (image != null)
            image.color = color;
    }
}