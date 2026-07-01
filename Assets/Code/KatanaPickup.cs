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

        Destroy(gameObject);
    }
}