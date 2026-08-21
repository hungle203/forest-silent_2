using UnityEngine;

public class MedkitPickup : MonoBehaviour, IInteractable
{
    public string GetInteractText()
    {
        return "Nhặt hộp cứu thương";
    }

    public void Interact()
    {
        if (MedkitInventory.Instance == null)
            return;

        MedkitInventory.Instance.PickupMedkit();

          if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPickup();
        }

        Destroy(gameObject);
    }
}