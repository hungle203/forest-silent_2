using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image fillImage;

    public float smoothSpeed = 3f;

    private float currentFill;

    private void Start()
    {
        currentFill = 1f;
    }

    private void Update()
    {
        float targetFill =
            (float)playerHealth.GetHealth() /
            playerHealth.maxHealth;

        currentFill = Mathf.Lerp(
            currentFill,
            targetFill,
            Time.deltaTime * smoothSpeed
        );

        fillImage.fillAmount = currentFill;

        float hpPercent =
    (float)playerHealth.GetHealth() /
    playerHealth.maxHealth;

fillImage.fillAmount = currentFill;

if (hpPercent > 0.6f)
{
    fillImage.color = Color.green;
}
else if (hpPercent > 0.3f)
{
    fillImage.color = Color.yellow;
}
else
{
    fillImage.color = Color.red;
}
    }
}