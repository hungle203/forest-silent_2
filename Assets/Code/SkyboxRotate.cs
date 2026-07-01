using UnityEngine;

public class SkyboxRotate : MonoBehaviour
{
    public float rotationSpeed = 0.5f;

    void Update()
    {
        RenderSettings.skybox.SetFloat(
            "_Rotation",
            Time.time * rotationSpeed);
    }
}