using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SwordAttack : MonoBehaviour
{
    private Animator anim;

    public Camera playerCamera;
    public float attackRange = 3f;
    public float damage = 25f;

    private bool attacking;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !attacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        attacking = true;

        anim.SetTrigger("Slash");

        // Chờ tới lúc kiếm chạm mục tiêu
        yield return new WaitForSeconds(0.25f);

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward);
if (Physics.Raycast(ray, out RaycastHit hit, attackRange))
{
    ZombieAI zombie = hit.collider.GetComponentInParent<ZombieAI>();

    if (zombie != null)
    {
        zombie.TakeDamage(damage);

        zombie.SpawnBlood(hit.point, hit.normal);

        Debug.Log("Hit Zombie");
    }
}

        yield return new WaitForSeconds(0.4f);

        attacking = false;
    }
}