using UnityEngine;

public class PaperPickup : MonoBehaviour, IInteractable
{
    public string GetInteractText()
    {
        return "Nhặt mảnh đá";
    }

    public void Interact()
    {
        UIManager.Instance.paperCount++;

        Destroy(gameObject);
    }
}