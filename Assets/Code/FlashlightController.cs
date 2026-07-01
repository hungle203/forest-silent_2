using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    public GameObject handFlashlight;
    public Light flashlightLight;

    public bool hasFlashlight;

    [Header("Battery")]
    public float maxBattery = 100f;
    public float currentBattery = 100f;
    public float drainRate = 2f;

    void Start()
    {
        handFlashlight.SetActive(false);
    }

    void Update()
    {
        if (!hasFlashlight)
            return;

        if (!handFlashlight.activeSelf)
            handFlashlight.SetActive(true);

        // Bật/tắt đèn
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (currentBattery > 0)
                flashlightLight.enabled = !flashlightLight.enabled;
        }

        // Hao pin
        if (flashlightLight.enabled)
        {
            currentBattery -= drainRate * Time.deltaTime;

            currentBattery = Mathf.Clamp(currentBattery, 0, maxBattery);

            if (currentBattery <= 0)
            {
                currentBattery = 0;
                flashlightLight.enabled = false;
            }
        }

        // Bấm T để thay pin
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            ReplaceBattery();
        }
    }

    void ReplaceBattery()
    {
        // Pin đang đầy thì không thay
        if (currentBattery >= maxBattery)
            return;

        // Không còn pin dự trữ
        if (UIManager.Instance.batteryCount <= 0)
            return;

        // Trừ 1 cục pin trong kho
        UIManager.Instance.batteryCount--;

        // Nạp đầy pin đèn
        currentBattery = maxBattery;

        Debug.Log("Đã thay pin");
    }

    public void PickupFlashlight()
    {
        hasFlashlight = true;

        handFlashlight.SetActive(true);
        flashlightLight.enabled = true;
    }
}