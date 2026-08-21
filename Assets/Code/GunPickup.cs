using UnityEngine;

public class GunPickup : MonoBehaviour, IInteractable
{
    public string GetInteractText()
    {
        return "Nhặt súng";
    }

    public void Interact()
    {
        // Âm thanh nhặt súng
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPickup();
        }

        // Nhặt súng
        WeaponManager.Instance.PickupGun();

        // Xóa súng trên mặt đất
        Destroy(gameObject);
    }
}