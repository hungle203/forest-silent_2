using UnityEngine;

public class KatanaPickup : MonoBehaviour, IInteractable
{
    public string GetInteractText()
    {
        return "Nhặt Katana";
    }

    public void Interact()
    {
        WeaponManager.Instance.PickupKatana();

         // Âm thanh kiếm 
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPickup();
        }

        Destroy(gameObject);
    }
}