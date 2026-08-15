using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class BossAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float attackRange = 2.2f;

    [Header("Attack")]
    public float attackDamage = 30f;
    public float attackCooldown = 1.5f;
    public float damageDelay = 0.7f;
    public float attackAnimationTime = 1.3f;

    [Header("Health")]
    public float maxHealth = 500f;

    private float currentHealth;
    private float nextAttackTime;

    private NavMeshAgent agent;
    private Animator anim;

    private bool attacking;
    private bool dead;

    void Start()
    {
        // =========================
        // GET COMPONENTS
        // =========================

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // =========================
        // HEALTH
        // =========================

        currentHealth = maxHealth;

        // =========================
        // FIND PLAYER
        // =========================

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
            {
                player = p.transform;
            }
            else
            {
                Debug.LogWarning("BossAI: Không tìm thấy Player có Tag = Player.");
            }
        }

        // =========================
        // NAVMESH
        // =========================

        agent.speed = moveSpeed;

        // Boss dừng ở khoảng cách này
        agent.stoppingDistance = attackRange * 0.8f;

        // Script tự xoay Boss
        agent.updateRotation = false;

        // =========================
        // ANIMATOR
        // =========================

        anim.SetBool("IsWalking", false);
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsDead", false);
    }

    void Update()
    {
        if (dead || player == null)
            return;

        // Nếu Boss không còn nằm trên NavMesh
        if (!agent.isOnNavMesh)
        {
            anim.SetBool("IsWalking", false);
            return;
        }

        // =========================
        // DISTANCE TO PLAYER
        // =========================

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        // =========================
        // ATTACK
        // =========================

        if (distance <= attackRange)
        {
            StopMovement();

            FacePlayer();

            if (!attacking && Time.time >= nextAttackTime)
            {
                StartCoroutine(Attack());
            }

            return;
        }

        // =========================
        // CHASE
        // =========================

        ChasePlayer();
    }

    // =========================================================
    // CHASE PLAYER
    // =========================================================

    void ChasePlayer()
    {
        if (!agent.isOnNavMesh)
            return;

        // Cho phép Boss di chuyển
        agent.isStopped = false;

        // Tìm đường đến Player
        agent.SetDestination(player.position);

        // Không attack trong lúc chạy
        anim.SetBool("IsAttacking", false);

        // =========================
        // WALK ANIMATION
        // =========================

        bool isMoving = agent.velocity.sqrMagnitude > 0.01f;

        anim.SetBool("IsWalking", isMoving);

        // =========================
        // ROTATION
        // =========================

        FacePlayer();
    }

    // =========================================================
    // STOP MOVEMENT
    // =========================================================

    void StopMovement()
    {
        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        anim.SetBool("IsWalking", false);
    }

    // =========================================================
    // FACE PLAYER
    // =========================================================

    void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            8f * Time.deltaTime
        );
    }

    // =========================================================
    // ATTACK
    // =========================================================

    IEnumerator Attack()
    {
        attacking = true;

        nextAttackTime = Time.time + attackCooldown;

        // Dừng Boss
        StopMovement();

        // Đảm bảo Boss nhìn Player
        FacePlayer();

        // Chạy Attack animation
        anim.SetBool("IsWalking", false);
        anim.SetBool("IsAttacking", true);

        // =========================
        // DAMAGE DELAY
        // =========================

        yield return new WaitForSeconds(damageDelay);

        if (!dead)
        {
            DealDamage();
        }

        // =========================
        // CHỜ ANIMATION
        // =========================

        yield return new WaitForSeconds(attackAnimationTime);

        if (!dead)
        {
            anim.SetBool("IsAttacking", false);
        }

        attacking = false;
    }

    // =========================================================
    // DEAL DAMAGE
    // =========================================================

    void DealDamage()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        // Player đã chạy ra ngoài tầm đánh
        if (distance > attackRange + 0.5f)
            return;

        PlayerHealth health =
            player.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(attackDamage);
        }
    }

    // =========================================================
    // TAKE DAMAGE
    // =========================================================

    public void TakeDamage(float damage)
    {
        if (dead)
            return;

        currentHealth -= damage;

        Debug.Log(
            "Boss HP: " + currentHealth
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // =========================================================
    // DIE
    // =========================================================

    void Die()
    {
        if (dead)
            return;

        dead = true;

        StopAllCoroutines();

        // Dừng NavMesh
        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }

        agent.enabled = false;

        // Tắt các animation khác
        anim.SetBool("IsWalking", false);
        anim.SetBool("IsAttacking", false);

        // Chạy Dead
        anim.SetBool("IsDead", true);

        // Xóa Boss sau 10 giây
        Destroy(gameObject, 10f);
    }
}