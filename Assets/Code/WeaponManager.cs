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

        // Ban đầu chưa có bộ đàm
        if (walkieTalkie != null)
            walkieTalkie.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            EquipGun();

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            EquipKatana();

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            EquipNone();
    }

    // ===== PICKUP =====

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

    // ===== EQUIP =====

   public void EquipGun()
{
    if (!hasGun) return;

    currentWeapon = WeaponType.Gun;

    gun.SetActive(true);
    katana.SetActive(false);

    InventoryUI.Instance.SelectGun();
}

   public void EquipKatana()
{
    if (!hasKatana) return;

    currentWeapon = WeaponType.Katana;

    gun.SetActive(false);
    katana.SetActive(true);

    InventoryUI.Instance.SelectKatana();
}

  public void EquipNone()
{
    currentWeapon = WeaponType.None;

    gun.SetActive(false);
    katana.SetActive(false);

    InventoryUI.Instance.SelectNone();
}

    void UnequipAll()
    {
        currentWeapon = WeaponType.None;

        gun.SetActive(false);
        katana.SetActive(false);
    }
}