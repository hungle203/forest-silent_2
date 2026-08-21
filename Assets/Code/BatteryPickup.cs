using UnityEngine;

public class BatteryPickup : MonoBehaviour, IInteractable
{
    public string GetInteractText()
    {
        return "Nhặt pin";
    }

    public void Interact()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.AddBatteryCount(1);
        }


          if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPickup();
        }

        Destroy(gameObject);
    }
}