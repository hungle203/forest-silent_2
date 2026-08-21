using UnityEngine;

public class FlashlightPickup : MonoBehaviour, IInteractable
{
    public string GetInteractText()
    {
        return "Nhặt đèn pin";
    }

    public void Interact()
    {
        FlashlightController flash =
            FindFirstObjectByType<FlashlightController>();

        if (flash != null)
        {
            flash.PickupFlashlight();
        }

         // Âm thanh nhặt đèn pin 
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPickup();
        }

        Destroy(gameObject);
    }
}