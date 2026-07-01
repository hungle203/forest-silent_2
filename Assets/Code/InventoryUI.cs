using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    Color activeColor = Color.white;
    Color inactiveColor = new Color(1, 1, 1, 0.3f);

    [Header("Slots")]
    public GameObject katanaSlot;
    public GameObject gunSlot;

    [Header("Slot Images")]
    public Image katanaImage;
    public Image gunImage;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        katanaSlot.SetActive(false);
        gunSlot.SetActive(false);
    }

    public void ShowKatana()
    {
        katanaSlot.SetActive(true);
    }

    public void ShowGun()
    {
        gunSlot.SetActive(true);
    }

    public void SelectGun()
    {
        gunImage.color = activeColor;
        katanaImage.color = inactiveColor;
    }

    public void SelectKatana()
    {
        katanaImage.color = activeColor;
        gunImage.color = inactiveColor;
    }

    public void SelectNone()
    {
        gunImage.color = inactiveColor;
        katanaImage.color = inactiveColor;
    }
}