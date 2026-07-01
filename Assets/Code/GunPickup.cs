using UnityEngine;

public class GunPickup : MonoBehaviour, IInteractable
{
    public string GetInteractText()
    {
        return "Nhặt súng";
    }

    public void Interact()
    {
        WeaponManager.Instance.PickupGun();

        Destroy(gameObject);
    }
}