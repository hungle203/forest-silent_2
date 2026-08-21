using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class BossAI : MonoBehaviour
{
    // =========================================================
    // TARGET
    // =========================================================

    [Header("Target")]
    public Transform player;


    // =========================================================
    // MOVEMENT
    // =========================================================

    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float attackRange = 2.2f;


    // =========================================================
    // ATTACK
    // =========================================================

    [Header("Attack")]
    public float attackDamage = 30f;
    public float attackCooldown = 1.5f;
    public float damageDelay = 0.7f;
    public float attackAnimationTime = 1.3f;


    // =========================================================
    // HEALTH
    // =========================================================

    [Header("Health")]
    public float maxHealth = 500f;


    // =========================================================
    // BOSS AUDIO
    // =========================================================

    [Header("Boss Audio")]

    public AudioSource audioSource;

    [Tooltip("Tiếng gào / gầm")]
    public AudioClip[] roarSounds;

    [Tooltip("Tiếng tấn công")]
    public AudioClip[] attackSounds;

    [Tooltip("Tiếng Boss bị đánh")]
    public AudioClip[] hurtSounds;

    [Tooltip("Tiếng Boss chết")]
    public AudioClip[] deathSounds;

    [Tooltip("Tiếng idle")]
    public AudioClip[] idleSounds;

    [Range(0f, 1f)]
    public float bossVolume = 1f;


    // =========================================================
    // 3D AUDIO
    // =========================================================

    [Header("3D Audio")]

    [Range(0f, 1f)]
    public float spatialBlend = 1f;

    public float minDistance = 3f;
    public float maxDistance = 25f;


    // =========================================================
    // IDLE SOUND
    // =========================================================

    [Header("Idle Sound")]

    public float idleSoundMinDelay = 5f;
    public float idleSoundMaxDelay = 10f;

    private float nextIdleSound;


    // =========================================================
    // AUDIO STATE
    // =========================================================

    // FALSE = Boss chưa được phép phát âm thanh
    // TRUE  = Cutscene đã kết thúc, Boss được phát âm thanh
    private bool bossAudioEnabled = false;


    // =========================================================
    // VARIABLES
    // =========================================================

    private float currentHealth;
    private float nextAttackTime;

    private NavMeshAgent agent;
    private Animator anim;

    private bool attacking = false;
    private bool dead = false;


    // =========================================================
    // START
    // =========================================================

    void Start()
    {
        // =====================================================
        // COMPONENTS
        // =====================================================

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();


        // =====================================================
        // AUDIO SOURCE
        // =====================================================

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }


        // =====================================================
        // AUDIO CONFIG
        // =====================================================

        audioSource.playOnAwake = false;
        audioSource.loop = false;

        audioSource.spatialBlend = spatialBlend;

        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;

        audioSource.rolloffMode =
            AudioRolloffMode.Logarithmic;

        audioSource.volume = 1f;

        audioSource.Stop();


        // =====================================================
        // HEALTH
        // =====================================================

        currentHealth = maxHealth;


        // =====================================================
        // FIND PLAYER
        // =====================================================

        if (player == null)
        {
            GameObject p =
                GameObject.FindGameObjectWithTag("Player");

            if (p != null)
            {
                player = p.transform;
            }
            else
            {
                Debug.LogWarning(
                    "BossAI: Không tìm thấy Player có Tag = Player."
                );
            }
        }


        // =====================================================
        // NAVMESH
        // =====================================================

        agent.speed = moveSpeed;

        agent.stoppingDistance =
            attackRange * 0.8f;

        // Boss tự xoay
        agent.updateRotation = false;


        // =====================================================
        // ANIMATOR
        // =====================================================

        anim.SetBool("IsWalking", false);
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsDead", false);


        // =====================================================
        // AUDIO BAN ĐẦU TẮT
        // =====================================================

        bossAudioEnabled = false;

        SetNextIdleSound();

        Debug.Log(
            "BossAI: Boss Audio đang TẮT - chờ Cutscene kết thúc."
        );
    }


    // =========================================================
    // ENABLE BOSS AUDIO
    // =========================================================

    public void EnableBossAudio()
    {
        if (dead)
            return;

        bossAudioEnabled = true;

        SetNextIdleSound();

        Debug.Log(
            "================================="
        );

        Debug.Log(
            "=== BOSS AUDIO ENABLED ==="
        );

        Debug.Log(
            "================================="
        );


        // =====================================================
        // BOSS GÀO NGAY KHI BẮT ĐẦU CHIẾN ĐẤU
        // =====================================================

        PlayRoar();
    }


    // =========================================================
    // DISABLE BOSS AUDIO
    // =========================================================

    public void DisableBossAudio()
    {
        bossAudioEnabled = false;

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        Debug.Log(
            "=== BOSS AUDIO DISABLED ==="
        );
    }


    // =========================================================
    // UPDATE
    // =========================================================

    void Update()
    {
        if (dead || player == null)
            return;


        // =====================================================
        // IDLE SOUND
        // =====================================================

        if (!attacking)
        {
            PlayIdleSound();
        }


        // =====================================================
        // NAVMESH
        // =====================================================

        if (!agent.isOnNavMesh)
        {
            anim.SetBool(
                "IsWalking",
                false
            );

            return;
        }


        // =====================================================
        // DISTANCE
        // =====================================================

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );


        // =====================================================
        // ATTACK
        // =====================================================

        if (distance <= attackRange)
        {
            StopMovement();

            FacePlayer();

            if (!attacking &&
                Time.time >= nextAttackTime)
            {
                StartCoroutine(Attack());
            }

            return;
        }


        // =====================================================
        // CHASE
        // =====================================================

        ChasePlayer();
    }


    // =========================================================
    // CHASE PLAYER
    // =========================================================

    void ChasePlayer()
    {
        if (!agent.isOnNavMesh)
            return;

        agent.isStopped = false;

        agent.SetDestination(
            player.position
        );


        // =====================================================
        // ANIMATION
        // =====================================================

        anim.SetBool(
            "IsAttacking",
            false
        );

        bool isMoving =
            agent.velocity.sqrMagnitude > 0.01f;

        anim.SetBool(
            "IsWalking",
            isMoving
        );


        // =====================================================
        // ROTATION
        // =====================================================

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

        anim.SetBool(
            "IsWalking",
            false
        );
    }


    // =========================================================
    // FACE PLAYER
    // =========================================================

    void FacePlayer()
    {
        if (player == null)
            return;

        Vector3 direction =
            player.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation =
            Quaternion.Slerp(
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

        nextAttackTime =
            Time.time +
            attackCooldown;

        StopMovement();

        FacePlayer();


        // =====================================================
        // ANIMATION
        // =====================================================

        anim.SetBool(
            "IsWalking",
            false
        );

        anim.SetBool(
            "IsAttacking",
            true
        );


        // =====================================================
        // ATTACK SOUND
        // =====================================================

        PlayRandomSound(
            attackSounds
        );


        // =====================================================
        // DAMAGE DELAY
        // =====================================================

        yield return new WaitForSeconds(
            damageDelay
        );


        if (!dead)
        {
            DealDamage();
        }


        // =====================================================
        // WAIT ATTACK ANIMATION
        // =====================================================

        yield return new WaitForSeconds(
            attackAnimationTime
        );


        if (!dead)
        {
            anim.SetBool(
                "IsAttacking",
                false
            );
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

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distance >
            attackRange + 0.5f)
            return;


        PlayerHealth health =
            player.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(
                attackDamage
            );
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
            "Boss HP: " +
            currentHealth
        );


        // =====================================================
        // HURT SOUND
        // =====================================================

        PlayRandomSound(
            hurtSounds
        );


        // =====================================================
        // DEAD
        // =====================================================

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


        // =====================================================
        // DEATH SOUND
        // =====================================================

        PlayRandomSound(
            deathSounds
        );


        // =====================================================
        // STOP NAVMESH
        // =====================================================

        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }

        agent.enabled = false;


        // =====================================================
        // ANIMATION
        // =====================================================

        anim.SetBool(
            "IsWalking",
            false
        );

        anim.SetBool(
            "IsAttacking",
            false
        );

        anim.SetBool(
            "IsDead",
            true
        );


        // =====================================================
        // DESTROY BOSS
        // =====================================================

        Destroy(
            gameObject,
            10f
        );
    }


    // =========================================================
    // IDLE SOUND
    // =========================================================

    void PlayIdleSound()
    {
        // Chưa kết thúc cutscene
        // => KHÔNG PHÁT ÂM THANH

        if (!bossAudioEnabled)
            return;


        if (idleSounds == null ||
            idleSounds.Length == 0)
            return;


        if (Time.time >= nextIdleSound)
        {
            PlayRandomSound(
                idleSounds
            );

            SetNextIdleSound();
        }
    }


    // =========================================================
    // SET NEXT IDLE SOUND
    // =========================================================

    void SetNextIdleSound()
    {
        nextIdleSound =
            Time.time +
            Random.Range(
                idleSoundMinDelay,
                idleSoundMaxDelay
            );
    }


    // =========================================================
    // ROAR
    // =========================================================

    public void PlayRoar()
    {
        PlayRandomSound(
            roarSounds
        );
    }


    // =========================================================
    // RANDOM SOUND
    // =========================================================

    void PlayRandomSound(
        AudioClip[] sounds
    )
    {
        // =====================================================
        // QUAN TRỌNG
        // =====================================================
        // Nếu Cutscene chưa kết thúc
        // => KHÔNG PHÁT BẤT KỲ BOSS SFX NÀO

        if (!bossAudioEnabled)
            return;


        if (sounds == null ||
            sounds.Length == 0)
            return;


        if (audioSource == null)
            return;


        AudioClip clip =
            sounds[
                Random.Range(
                    0,
                    sounds.Length
                )
            ];


        if (clip == null)
            return;


        audioSource.PlayOneShot(
            clip,
            bossVolume
        );


        Debug.Log(
            "Boss SFX: " +
            clip.name
        );
    }
}