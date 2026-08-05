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

    [Header("Blood")]
    public GameObject bloodEffect;

    [Header("Performance")]
    [Tooltip("AI cập nhật bao nhiêu lần mỗi giây")]
    public float aiUpdateRate = 10f;

    float currentHealth;

    NavMeshAgent agent;
    Animator anim;

    bool playerDetected;
    bool dead;
    bool attacking;
    bool roaring;
    bool gettingHit;

    float aiTimer;

    // Cache khoảng cách bình phương
    float detectRangeSqr;
    float runRangeSqr;
    float attackRangeSqr;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;

        // Cache để không phải tính lại mỗi frame
        detectRangeSqr = detectRange * detectRange;
        runRangeSqr = runRange * runRange;
        attackRangeSqr = attackRange * attackRange;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
                player = p.transform;
        }

        aiTimer = Random.Range(0f, 0.1f);
    }

    void Update()
    {
        if (dead || player == null)
            return;

        // Không cần tính AI mỗi frame
        aiTimer -= Time.deltaTime;

        if (aiTimer > 0f)
            return;

        aiTimer = 1f / aiUpdateRate;

        UpdateAI();
    }

    void UpdateAI()
    {
        if (attacking || roaring || gettingHit)
            return;

        Vector3 offset = transform.position - player.position;

        float distanceSqr = offset.sqrMagnitude;

        //--------------------------------
        // Phát hiện player
        //--------------------------------
        if (!playerDetected)
        {
            if (distanceSqr <= detectRangeSqr)
            {
                playerDetected = true;

                StartCoroutine(RoarCoroutine());
            }

            return;
        }

        //--------------------------------
        // Tấn công
        //--------------------------------
        if (distanceSqr <= attackRangeSqr)
        {
            StartCoroutine(AttackCoroutine());
            return;
        }

        //--------------------------------
        // Đuổi player
        //--------------------------------
        if (!agent.enabled)
            return;

        agent.isStopped = false;

        // Chỉ SetDestination khi cần
        agent.SetDestination(player.position);

        //--------------------------------
        // Walk / Run
        //--------------------------------
        if (distanceSqr <= runRangeSqr)
        {
            agent.speed = runSpeed;
            anim.SetFloat("Speed", 1f);
        }
        else
        {
            agent.speed = walkSpeed;
            anim.SetFloat("Speed", 0.5f);
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

        // Quay mặt về player
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            transform.rotation =
                Quaternion.LookRotation(dir);
        }

        int randomAttack = Random.Range(1, 5);

        anim.CrossFade(
            "attack" + randomAttack,
            0.1f
        );

        // Chờ animation tới lúc đánh
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

        if (currentHealth <= 0f)
        {
            Die();
            return;
        }

        StartCoroutine(GetHitCoroutine());
    }

    IEnumerator GetHitCoroutine()
    {
        gettingHit = true;

        if (agent.enabled)
            agent.isStopped = true;

        anim.CrossFade("gethit", 0.1f);

        yield return new WaitForSeconds(0.6f);

        gettingHit = false;
    }

    void Die()
    {
        dead = true;

        StopAllCoroutines();

        if (agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        int randomDeath = Random.Range(1, 3);

        anim.CrossFade(
            "death" + randomDeath,
            0.1f
        );

        Destroy(gameObject, 10f);
    }

    public void DealDamage()
    {
        if (player == null)
            return;

        Vector3 offset = transform.position - player.position;

        float distanceSqr = offset.sqrMagnitude;

        float damageRange = attackRange + 0.5f;

        if (distanceSqr <= damageRange * damageRange)
        {
            PlayerHealth hp =
                player.GetComponent<PlayerHealth>();
if (hp != null)
{
    hp.TakeDamage(attackDamage);

    DamageVignette vignette =
        FindFirstObjectByType<DamageVignette>();

    if (vignette != null)
    {
        vignette.ShowDamage();
    }
}
        }
    }

    // ========================================
    // BLOOD
    // ========================================

    public void SpawnBlood(
        Vector3 hitPoint,
        Vector3 hitNormal)
    {
        if (bloodEffect == null)
            return;

        GameObject blood = Instantiate(
            bloodEffect,
            hitPoint,
            Quaternion.LookRotation(hitNormal)
        );

        Destroy(blood, 2f);
    }

    // ========================================
    // GIZMOS
    // ========================================

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            detectRange
        );

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(
            transform.position,
            runRange
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );

        if (player != null)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(
                transform.position,
                player.position
            );
        }
    }
}