using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MicrophoneUI : MonoBehaviour
{
    public Slider micSlider;
    public TMP_Text micText;
    public Image fillImage; // Fill của Slider

    private MicrophoneManager mic;

    void Start()
    {
        mic = FindFirstObjectByType<MicrophoneManager>();

        if (mic == null)
        {
            Debug.LogError("Không tìm thấy MicrophoneManager!");
            return;
        }

        micSlider.maxValue = mic.screamThreshold;
    }

    void Update()
    {
        if (mic == null)
            return;

        micSlider.value = mic.loudness;

        micText.text = mic.loudness.ToString("F1");

        // Đổi màu thanh mic
        float percent = mic.loudness / mic.screamThreshold;

        if (percent < 0.5f)
        {
            fillImage.color = Color.green;
        }
        else if (percent < 0.8f)
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Color.red;
        }
    }
}