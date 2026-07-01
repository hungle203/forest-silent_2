using UnityEngine;

public class TreeWind : MonoBehaviour
{
    public float swayAmount = 2f;     // độ rung
    public float swaySpeed = 1f;      // tốc độ

    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swaySpeed) * swayAmount;

        transform.localRotation =
            startRotation * Quaternion.Euler(0, angle, 0);
    }
}