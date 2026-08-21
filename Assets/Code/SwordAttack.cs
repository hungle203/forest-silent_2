using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SwordAttack : MonoBehaviour
{
    private Animator anim;

    [Header("Camera")]
    public Camera playerCamera;

    [Header("Attack")]
    public float attackRange = 3f;
    public float damage = 25f;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip slashSound;

    private bool attacking;

    private void Start()
    {
        anim = GetComponent<Animator>();

        // Nếu chưa kéo AudioSource vào Inspector
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame && !attacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        attacking = true;

        // =========================
        // ANIMATION CHÉM
        // =========================

        anim.SetTrigger("Slash");

        // =========================
        // ÂM THANH CHÉM
        // =========================

        if (audioSource != null && slashSound != null)
        {
            audioSource.PlayOneShot(slashSound);
        }

        // =========================
        // CHỜ KIẾM CHẠM
        // =========================

        yield return new WaitForSeconds(0.25f);

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            attackRange))
        {
            ZombieAI zombie =
                hit.collider.GetComponentInParent<ZombieAI>();

            if (zombie != null)
            {
                zombie.TakeDamage(damage);

                zombie.SpawnBlood(
                    hit.point,
                    hit.normal
                );

                Debug.Log("Hit Zombie");
            }
        }

        // =========================
        // KẾT THÚC ATTACK
        // =========================

        yield return new WaitForSeconds(0.4f);

        attacking = false;
    }
}