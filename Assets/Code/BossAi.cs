using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
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
    // BLOOD EFFECT
    // =========================================================

    [Header("Blood Effect")]
    public GameObject bloodEffect;
    public Transform bloodSpawnPoint;
    public float bloodEffectDestroyTime = 2f;


    // =========================================================
    // HIT REACTION
    // =========================================================

    [Header("Hit Reaction")]
    public float hitStunTime = 0.35f;
    public float knockbackDistance = 0.8f;
    public float knockbackSpeed = 5f;


    // =========================================================
    // BOSS AUDIO
    // =========================================================

    [Header("Boss Audio")]

    public AudioSource audioSource;

    [Tooltip("Tiếng gào / gầm")]
    public AudioClip[] roarSounds;

    [Tooltip("Tiếng tấn công")]
    public AudioClip[] attackSounds;

    [Tooltip("Tiếng bị đánh")]
    public AudioClip[] hurtSounds;

    [Tooltip("Tiếng chết")]
    public AudioClip[] deathSounds;

    [Tooltip("Tiếng bước chân")]
    public AudioClip[] footstepSounds;

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

    public float minDistance = 5f;
    public float maxDistance = 50f;


    // =========================================================
    // IDLE AUDIO
    // =========================================================

    [Header("Idle Sound")]

    public float idleSoundMinDelay = 5f;
    public float idleSoundMaxDelay = 10f;

    private float nextIdleSound;


    // =========================================================
    // GAMEPLAY STATE
    // =========================================================

    private bool gameplayEnabled = false;


    // =========================================================
    // COMPONENTS
    // =========================================================

    private NavMeshAgent agent;
    private Animator anim;


    // =========================================================
    // STATE
    // =========================================================

    private float currentHealth;
    private float nextAttackTime;

    private bool attacking;
    private bool dead;
    private bool hitStunned;


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

        audioSource.volume = bossVolume;

        audioSource.spatialBlend = spatialBlend;

        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;

        audioSource.rolloffMode =
            AudioRolloffMode.Logarithmic;

        audioSource.enabled = true;


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
                    "BossAI: Không tìm thấy Player!"
                );
            }
        }


        // =====================================================
        // NAVMESH
        // =====================================================

        agent.speed = moveSpeed;

        agent.stoppingDistance =
            attackRange * 0.8f;

        agent.updateRotation = false;


        // =====================================================
        // ANIMATOR
        // =====================================================

        anim.SetBool("IsWalking", false);
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsDead", false);


        // =====================================================
        // IDLE TIMER
        // =====================================================

        SetNextIdleSound();


        // =====================================================
        // GAMEPLAY BAN ĐẦU TẮT
        // =====================================================

        gameplayEnabled = false;


        Debug.Log("=================================");
        Debug.Log("=== BOSS AI READY ===");
        Debug.Log("=== BOSS AUDIO READY ===");
        Debug.Log("=== BOSS GAMEPLAY OFF ===");
        Debug.Log("=================================");
    }


    // =========================================================
    // ENABLE BOSS AUDIO
    // =========================================================

    public void EnableBossAudio()
    {
        if (dead)
            return;

        if (audioSource != null)
        {
            audioSource.enabled = true;
            audioSource.volume = bossVolume;
        }

        SetNextIdleSound();

        Debug.Log("=== BOSS AUDIO ENABLED ===");
    }


    // =========================================================
    // DISABLE BOSS AUDIO
    // =========================================================

    public void DisableBossAudio()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        Debug.Log("=== BOSS AUDIO STOPPED ===");
    }


    // =========================================================
    // SET GAMEPLAY ENABLED
    // =========================================================

    public void SetGameplayEnabled(bool enabled)
    {
        gameplayEnabled = enabled;

        if (!enabled)
        {
            attacking = false;

            if (agent != null &&
                agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }

            if (anim != null)
            {
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsAttacking", false);
            }

            Debug.Log(
                "=== BOSS GAMEPLAY DISABLED ==="
            );
        }
        else
        {
            if (agent != null &&
                agent.isOnNavMesh)
            {
                agent.isStopped = false;
            }

            Debug.Log(
                "=== BOSS GAMEPLAY ENABLED ==="
            );
        }
    }


    // =========================================================
    // ROAR FROM CUTSCENE
    // =========================================================

    public void PlayRoarFromCutscene()
    {
        if (dead)
            return;

        Debug.Log(
            "=== BOSS ROAR FROM TIMELINE ==="
        );

        PlayRandomSound(roarSounds);
    }


    // =========================================================
    // ROAR
    // =========================================================

    public void PlayRoar()
    {
        if (dead)
            return;

        PlayRandomSound(roarSounds);
    }


    // =========================================================
    // UPDATE
    // =========================================================

    void Update()
    {
        if (dead)
            return;


        // =====================================================
        // GAMEPLAY OFF
        // =====================================================

        if (!gameplayEnabled)
            return;


        if (player == null)
            return;


        // =====================================================
        // HIT STUN
        // =====================================================

        if (hitStunned)
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
                StartCoroutine(
                    Attack()
                );
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
        if (!gameplayEnabled)
            return;

        if (!agent.isOnNavMesh)
            return;

        agent.isStopped = false;

        agent.SetDestination(
            player.position
        );

        anim.SetBool(
            "IsAttacking",
            false
        );

        bool isMoving =
            agent.velocity.sqrMagnitude >
            0.01f;

        anim.SetBool(
            "IsWalking",
            isMoving
        );

        FacePlayer();
    }


    // =========================================================
    // STOP MOVEMENT
    // =========================================================

    void StopMovement()
    {
        if (agent != null &&
            agent.isOnNavMesh)
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
            Quaternion.LookRotation(
                direction
            );

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
        if (!gameplayEnabled)
            yield break;

        attacking = true;

        nextAttackTime =
            Time.time +
            attackCooldown;

        StopMovement();

        FacePlayer();

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


        if (!dead &&
            gameplayEnabled)
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
            "BOSS TAKE DAMAGE: " +
            damage
        );

        Debug.Log(
            "BOSS HP: " +
            currentHealth +
            " / " +
            maxHealth
        );


        // =====================================================
        // BLOOD
        // =====================================================

        SpawnBloodEffect();


        // =====================================================
        // HURT SOUND
        // =====================================================

        PlayRandomSound(
            hurtSounds
        );


        // =====================================================
        // HIT REACTION
        // =====================================================

        if (!hitStunned)
        {
            StartCoroutine(
                HitReaction()
            );
        }


        // =====================================================
        // DEATH
        // =====================================================

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    // =========================================================
    // BLOOD EFFECT
    // =========================================================

    void SpawnBloodEffect()
    {
        if (bloodEffect == null)
            return;

        Vector3 spawnPosition;

        if (bloodSpawnPoint != null)
        {
            spawnPosition =
                bloodSpawnPoint.position;
        }
        else
        {
            spawnPosition =
                transform.position +
                Vector3.up * 1.2f;
        }

        GameObject effect =
            Instantiate(
                bloodEffect,
                spawnPosition,
                Quaternion.identity
            );

        Destroy(
            effect,
            bloodEffectDestroyTime
        );
    }


    // =========================================================
    // HIT REACTION
    // =========================================================

    IEnumerator HitReaction()
    {
        if (dead)
            yield break;

        hitStunned = true;

        attacking = false;


        // =====================================================
        // STOP NAVMESH
        // =====================================================

        if (agent != null &&
            agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }


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


        // =====================================================
        // KNOCKBACK DIRECTION
        // =====================================================

        Vector3 direction =
            transform.position -
            player.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
        {
            direction =
                -transform.forward;
        }

        direction.Normalize();


        // =====================================================
        // KNOCKBACK
        // =====================================================

        Vector3 startPosition =
            transform.position;

        Vector3 targetPosition =
            startPosition +
            direction *
            knockbackDistance;

        float elapsed = 0f;


        while (elapsed < hitStunTime)
        {
            if (dead)
                yield break;

            elapsed +=
                Time.deltaTime;

            float t =
                elapsed /
                hitStunTime;

            Vector3 newPosition =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );

            newPosition.y =
                transform.position.y;

            if (agent != null &&
                agent.isOnNavMesh)
            {
                agent.Move(
                    newPosition -
                    transform.position
                );
            }
            else
            {
                transform.position =
                    newPosition;
            }

            yield return null;
        }


        // =====================================================
        // END HIT
        // =====================================================

        if (!dead &&
            gameplayEnabled &&
            agent != null &&
            agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }

        hitStunned = false;
    }


    // =========================================================
    // DIE
    // =========================================================

    void Die()
    {
        if (dead)
            return;


        // =====================================================
        // DEATH SOUND TRƯỚC KHI DEAD
        // =====================================================

        PlayRandomSound(
            deathSounds
        );


        dead = true;

        StopAllCoroutines();


        // =====================================================
        // STOP NAVMESH
        // =====================================================

        if (agent != null &&
            agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }

        if (agent != null)
        {
            agent.enabled = false;
        }


        // =====================================================
        // STOP ANIMATION
        // =====================================================

        anim.SetBool(
            "IsWalking",
            false
        );

        anim.SetBool(
            "IsAttacking",
            false
        );


        // =====================================================
        // DEAD ANIMATION
        // =====================================================

        anim.SetBool(
            "IsDead",
            true
        );


        // =====================================================
        // DESTROY
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
    // SET IDLE TIMER
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
    // FOOTSTEP
    // =========================================================

    public void PlayFootstep()
    {
        PlayRandomSound(
            footstepSounds
        );
    }


    // =========================================================
    // RANDOM SOUND
    // =========================================================

    void PlayRandomSound(
        AudioClip[] sounds
    )
    {
        if (sounds == null ||
            sounds.Length == 0)
        {
            Debug.LogWarning(
                "BossAI: AudioClip array đang trống!"
            );

            return;
        }


        if (audioSource == null)
        {
            Debug.LogWarning(
                "BossAI: AudioSource chưa được gán!"
            );

            return;
        }


        if (!audioSource.enabled)
        {
            audioSource.enabled = true;
        }


        AudioClip clip =
            sounds[
                Random.Range(
                    0,
                    sounds.Length
                )
            ];


        if (clip == null)
        {
            Debug.LogWarning(
                "BossAI: AudioClip bị NULL!"
            );

            return;
        }


        audioSource.volume =
            bossVolume;


        audioSource.PlayOneShot(
            clip,
            bossVolume
        );


        Debug.Log(
            "=== BOSS SOUND PLAY: " +
            clip.name +
            " ==="
        );
    }
}