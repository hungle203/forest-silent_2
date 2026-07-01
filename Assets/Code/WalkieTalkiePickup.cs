using UnityEngine;

public class WalkieTalkiePickup : MonoBehaviour, IInteractable
{
    public string speaker = "Canh tháp";

    public string GetInteractText()
    {
        return "Nhặt bộ đàm";
    }

    public void Interact()
    {
        WeaponManager.Instance.PickupWalkieTalkie();

        DialogueManager.Instance.speakerName = speaker;
        DialogueManager.Instance.StartDialogue();

        Destroy(gameObject);
    }
}