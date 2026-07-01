using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject panel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    [Header("Default Dialogue")]
    public string speakerName = "???";

    [TextArea(2,5)]
    public string[] dialogues;

    int currentIndex;
    bool isTalking;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    void Update()
    {
        if (!isTalking)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            NextDialogue();
        }
    }

    // Bắt đầu bằng hội thoại truyền vào
    public void StartDialogue(string speaker, string[] lines)
    {
        speakerName = speaker;
        dialogues = lines;

        currentIndex = 0;
        isTalking = true;

        panel.SetActive(true);

        ShowCurrentDialogue();
    }

    // Bắt đầu bằng hội thoại đã nhập trong Inspector
    public void StartDialogue()
    {
        currentIndex = 0;
        isTalking = true;

        panel.SetActive(true);

        ShowCurrentDialogue();
    }

    void ShowCurrentDialogue()
    {
        if (currentIndex >= dialogues.Length)
        {
            EndDialogue();
            return;
        }

        nameText.text = speakerName;
        dialogueText.text = dialogues[currentIndex];
    }

    void NextDialogue()
    {
        currentIndex++;

        if (currentIndex >= dialogues.Length)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentDialogue();
        }
    }

    public void EndDialogue()
{
    isTalking = false;
    panel.SetActive(false);

    // Tắt bộ đàm khi nói xong
    if (WeaponManager.Instance != null)
    {
        WeaponManager.Instance.HideWalkieTalkie();
    }
}
}