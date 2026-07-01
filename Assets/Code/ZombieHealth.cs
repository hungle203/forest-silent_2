using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float health = 100;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        anim.SetTrigger("Hit");

        if (health <= 0)
        {
            anim.SetTrigger("Die");

            GetComponent<ZombieAI>().enabled = false;

            Destroy(gameObject,5);
        }
    }
}