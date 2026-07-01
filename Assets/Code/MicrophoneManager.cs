using UnityEngine;

public class MicrophoneManager : MonoBehaviour
{
    [Header("Mic")]
    public float sensitivity = 100f;

    [Header("Result")]
    public float loudness;

    [Header("Threshold")]
    public float warningThreshold = 5f;
    public float screamThreshold = 15f;

    AudioClip micClip;
    string micDevice;

    const int sampleWindow = 256;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            micDevice = Microphone.devices[0];

            Debug.Log("Microphone: " + micDevice);

            micClip = Microphone.Start(
                micDevice,
                true,
                10,
                44100);
        }
        else
        {
            Debug.LogError("Không tìm thấy microphone!");
        }
    }

    void Update()
    {
        loudness = GetLoudness();

        if (loudness > warningThreshold)
        {
            Debug.Log("Cảnh báo");
        }

        if (loudness > screamThreshold)
        {
            Debug.Log("Player chết vì hét");

            //GameManager.Instance.PlayerDie();
        }
    }

    float GetLoudness()
    {
        if (micClip == null)
            return 0;

        int micPosition = Microphone.GetPosition(micDevice) - sampleWindow;

        if (micPosition < 0)
            return 0;

        float[] waveData = new float[sampleWindow];

        micClip.GetData(waveData, micPosition);

        float levelMax = 0;

        foreach (float sample in waveData)
        {
            float wavePeak = Mathf.Abs(sample);

            if (wavePeak > levelMax)
            {
                levelMax = wavePeak;
            }
        }

        return levelMax * sensitivity;
    }
}