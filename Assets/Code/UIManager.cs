using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI")]
    public Image flashlightIcon;
    public Slider batterySlider;
    public Image fillImage;

    [Header("Text")]
    public TMP_Text batteryCountText;
    public TMP_Text paperText;

    [Header("Ammo UI")]
    public GameObject ammoPanel;
    public TMP_Text ammoText;

    [Header("Data")]
    public int batteryCount;
    public int paperCount;

    private FlashlightController flash;
    private GunController gun;
    private WeaponManager weaponManager;

    private float currentSliderValue;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        flash = FindFirstObjectByType<FlashlightController>();
        gun = FindFirstObjectByType<GunController>();
        weaponManager = WeaponManager.Instance;

        if (flash != null)
        {
            batterySlider.maxValue = flash.maxBattery;
            currentSliderValue = flash.currentBattery;
        }

        // Ẩn UI đạn lúc bắt đầu
        if (ammoPanel != null)
            ammoPanel.SetActive(false);
    }

    void Update()
    {
        UpdateBatteryUI();
       
        UpdateAmmoUI();
    }

    void UpdateBatteryUI()
    {
        if (flash == null)
            return;

        // Icon đèn pin
        flashlightIcon.enabled = flash.hasFlashlight;

        // Pin tụt mượt
        currentSliderValue = Mathf.Lerp(
            currentSliderValue,
            flash.currentBattery,
            Time.deltaTime * 5f);

        batterySlider.value = currentSliderValue;

        batteryCountText.text = batteryCount.ToString();

        float percent = flash.currentBattery / flash.maxBattery;

        if (percent > 0.5f)
            fillImage.color = new Color(0.2f, 1f, 0.2f);
        else if (percent > 0.2f)
            fillImage.color = new Color(1f, 0.7f, 0.1f);
        else
            fillImage.color = new Color(1f, 0.1f, 0.1f);
    }

    

    void UpdateAmmoUI()
    {
        if (ammoPanel == null || ammoText == null)
            return;

        if (gun == null)
            gun = FindFirstObjectByType<GunController>();

        if (weaponManager == null)
            weaponManager = WeaponManager.Instance;

        bool showAmmo =
            weaponManager != null &&
            weaponManager.currentWeapon == WeaponManager.WeaponType.Gun;

        ammoPanel.SetActive(showAmmo);

        if (showAmmo && gun != null)
        {
            ammoText.text = gun.currentAmmo + "/" + gun.reserveAmmo;
        }
    }

    public void AddBatteryCount(int amount = 1)
    {
        batteryCount += amount;
    }

    public void AddPaperCount(int amount = 1)
    {
        paperCount += amount;
    }
}