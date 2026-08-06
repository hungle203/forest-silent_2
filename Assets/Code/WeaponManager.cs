using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    [Header("Weapons")]
    public GameObject gun;
    public GameObject katana;

    [Header("Items")]
    public GameObject walkieTalkie;

    [Header("Inventory")]
    public bool hasGun;
    public bool hasKatana;
    public bool hasWalkieTalkie;

    public enum WeaponType
    {
        None,
        Gun,
        Katana
    }

    public WeaponType currentWeapon = WeaponType.None;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UnequipAll();

        // Bộ đàm ban đầu ẩn
        if (walkieTalkie != null)
            walkieTalkie.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        // =========================
        // 1 = SÚNG
        // =========================
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            EquipGun();
        }

        // =========================
        // 2 = KATANA
        // =========================
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            EquipKatana();
        }

        // =========================
        // 3 = MEDKIT
        // =========================
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            UseMedkit();
        }

        // =========================
        // 4 = TAY KHÔNG
        // =========================
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            EquipNone();
        }
    }

    // ========================================
    // PICKUP
    // ========================================

    public void PickupGun()
    {
        hasGun = true;

        if (InventoryUI.Instance != null)
            InventoryUI.Instance.ShowGun();

        EquipGun();
    }

    public void PickupKatana()
    {
        hasKatana = true;

        if (InventoryUI.Instance != null)
            InventoryUI.Instance.ShowKatana();

        EquipKatana();
    }

    public void PickupWalkieTalkie()
    {
        hasWalkieTalkie = true;

        if (walkieTalkie != null)
            walkieTalkie.SetActive(true);
    }

    public void HideWalkieTalkie()
    {
        if (walkieTalkie != null)
            walkieTalkie.SetActive(false);
    }

    // ========================================
    // SÚNG
    // ========================================

    public void EquipGun()
    {
        if (!hasGun)
            return;

        currentWeapon = WeaponType.Gun;

        if (gun != null)
            gun.SetActive(true);

        if (katana != null)
            katana.SetActive(false);

        if (InventoryUI.Instance != null)
            InventoryUI.Instance.SelectGun();
    }

    // ========================================
    // KATANA
    // ========================================

    public void EquipKatana()
    {
        if (!hasKatana)
            return;

        currentWeapon = WeaponType.Katana;

        if (gun != null)
            gun.SetActive(false);

        if (katana != null)
            katana.SetActive(true);

        if (InventoryUI.Instance != null)
            InventoryUI.Instance.SelectKatana();
    }

    // ========================================
    // TAY KHÔNG
    // ========================================

    public void EquipNone()
    {
        currentWeapon = WeaponType.None;

        if (gun != null)
            gun.SetActive(false);

        if (katana != null)
            katana.SetActive(false);

        if (InventoryUI.Instance != null)
            InventoryUI.Instance.SelectNone();
    }

    // ========================================
    // DÙNG MEDKIT
    // ========================================

    void UseMedkit()
    {
        if (MedkitInventory.Instance == null)
            return;

        // Cất súng / kiếm
        EquipNone();

        // Dùng 1 hộp cứu thương
        MedkitInventory.Instance.UseMedkit();
    }

    // ========================================
    // BAN ĐẦU
    // ========================================

    void UnequipAll()
    {
        currentWeapon = WeaponType.None;

        if (gun != null)
            gun.SetActive(false);

        if (katana != null)
            katana.SetActive(false);
    }
}