using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageVignette : MonoBehaviour
{
    public Image damageImage;

    [Header("Effect")]
    public float maxAlpha = 0.8f;
    public float fadeSpeed = 3f;

    Coroutine damageCoroutine;

    void Start()
    {
        if (damageImage != null)
        {
            Color c = damageImage.color;
            c.a = 0f;
            damageImage.color = c;
        }
    }

    void Update()
    {
        if (damageImage == null)
            return;

        Color color = damageImage.color;

        if (color.a > 0f)
        {
            color.a = Mathf.MoveTowards(
                color.a,
                0f,
                fadeSpeed * Time.deltaTime
            );

            damageImage.color = color;
        }
    }

    public void ShowDamage()
    {
        if (damageImage == null)
            return;

        Color color = damageImage.color;
        color.a = maxAlpha;
        damageImage.color = color;
    }
}