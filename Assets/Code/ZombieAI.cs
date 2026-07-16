using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class ZombieAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Distance")]
    public float detectRange = 20f;
    public float runRange = 10f;
    public float attackRange = 2f;

    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;

    [Header("Stats")]
    public float maxHealth = 100f;

    [Header("Attack")]
public float attackDamage = 15f;

public GameObject bloodEffect;

    float currentHealth;

    NavMeshAgent agent;
    Animator anim;

    bool playerDetected;
    bool dead;
    bool attacking;
    bool roaring;
    bool gettingHit;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        if (dead || player == null)
            return;

        if (attacking || roaring || gettingHit)
            return;

        float distance =
            Vector3.Distance(transform.position, player.position);

        //--------------------------------
        // Phát hiện player
        //--------------------------------
        if (!playerDetected && distance <= detectRange)
        {
            playerDetected = true;
            StartCoroutine(RoarCoroutine());
            return;
        }

        if (!playerDetected)
            return;

        //--------------------------------
        // Tấn công
        //--------------------------------
        if (distance <= attackRange)
        {
            StartCoroutine(AttackCoroutine());
            return;
        }

        //--------------------------------
        // Đuổi player
        //--------------------------------
        agent.isStopped = false;
        agent.SetDestination(player.position);

        if (distance <= runRange)
        {
            agent.speed = runSpeed;
            anim.SetFloat("Speed", 1f); // Run
        }
        else
        {
            agent.speed = walkSpeed;
            anim.SetFloat("Speed", 0.5f); // Walk
        }
    }

    IEnumerator RoarCoroutine()
    {
        roaring = true;

        agent.isStopped = true;

        anim.CrossFade("roar", 0.15f);

        yield return new WaitForSeconds(2f);

        roaring = false;
    }

    IEnumerator AttackCoroutine()
    {
        attacking = true;

        agent.isStopped = true;

        // quay mặt về player
        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(dir),
                    1f);
        }

        int randomAttack = Random.Range(1, 5);

       anim.CrossFade("attack" + randomAttack, 0.1f);

// Chờ tới lúc tay zombie chạm người
yield return new WaitForSeconds(0.7f);

DealDamage();

yield return new WaitForSeconds(0.8f);

        attacking = false;
    }

    public void TakeDamage(float damage)
    {
        if (dead)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(GetHitCoroutine());
    }

    IEnumerator GetHitCoroutine()
    {
        gettingHit = true;

        agent.isStopped = true;

        anim.CrossFade("gethit", 0.1f);

        yield return new WaitForSeconds(0.6f);

        gettingHit = false;
    }

    void Die()
    {
        dead = true;

        agent.isStopped = true;
        agent.enabled = false;

        int randomDeath = Random.Range(1, 3);

        anim.CrossFade("death" + randomDeath, 0.1f);

        Destroy(gameObject, 10f);
    }

    public void DealDamage()
{
    if (player == null)
        return;

    float distance =
        Vector3.Distance(transform.position, player.position);

    if (distance <= attackRange + 0.5f)
    {
        PlayerHealth hp = player.GetComponent<PlayerHealth>();

        if (hp != null)
        {
            hp.TakeDamage(attackDamage);
        }
    }
}


void OnDrawGizmos()
{
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, detectRange);

    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, runRange);

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackRange);

    if (player != null)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, player.position);
    }
}

// spawn máu
public void SpawnBlood(Vector3 hitPoint, Vector3 hitNormal)
{
    if (bloodEffect == null) return;

    GameObject blood = Instantiate(
        bloodEffect,
        hitPoint,
        Quaternion.LookRotation(hitNormal));

    Destroy(blood, 2f);
}
}