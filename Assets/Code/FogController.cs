using UnityEngine;

public class FogController : MonoBehaviour
{
    [Header("Fog")]
    public Color fogColor = new Color(0.08f, 0.1f, 0.09f);
    public float fogDensity = 0.025f;

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = fogDensity;
    }
}