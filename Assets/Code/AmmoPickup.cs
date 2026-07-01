using UnityEngine;

public class AmmoPickup : MonoBehaviour, IInteractable
{
    public int ammoAmount = 30;

    public string GetInteractText()
    {
        return "Nhặt đạn";
    }

    public void Interact()
    {
        GunController gun = FindFirstObjectByType<GunController>();

        if (gun != null)
        {
            gun.AddAmmo(ammoAmount);
        }

        Destroy(gameObject);
    }
}