using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // =====================================================
    // AUDIO SOURCES
    // =====================================================

    [Header("Audio Sources")]

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource playerSource;
    public AudioSource zombieSource;
    public AudioSource bossSource;


    // =====================================================
    // BACKGROUND MUSIC
    // =====================================================

    [Header("Background Music")]

    public AudioClip gameplayMusic;
    public AudioClip bossFightMusic;


    // =====================================================
    // PLAYER
    // =====================================================

    [Header("Player - Footstep")]

    public AudioClip[] footstepSounds;


    [Header("Player - Weapons")]

    public AudioClip swordSlash;
    public AudioClip gunShot;
    public AudioClip reload;
    public AudioClip weaponSwitch;


    [Header("Player - Actions")]

    public AudioClip heal;
    public AudioClip pickup;


    [Header("Player - Breathing")]

    public AudioClip breathing;


    // =====================================================
    // ZOMBIE
    // =====================================================

    [Header("Zombie")]

    public AudioClip zombieIdle;
    public AudioClip zombieAttack;
    public AudioClip zombieHit;
    public AudioClip zombieDeath;
    public AudioClip[] zombieFootsteps;


    // =====================================================
    // BOSS
    // =====================================================

    [Header("Boss")]

    public AudioClip bossRoar;
    public AudioClip bossAttack;
    public AudioClip bossHit;
    public AudioClip bossDeath;
    public AudioClip[] bossFootsteps;


    // =====================================================
    // BOSS AUDIO STATE
    // =====================================================

    private bool bossAudioEnabled = false;


    // =====================================================
    // AWAKE
    // =====================================================

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }


    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        // Gameplay music chạy bình thường
        PlayMusic(gameplayMusic);

        // Boss audio luôn tắt khi game bắt đầu
        DisableBossAudio();
    }


    // =====================================================
    // MUSIC
    // =====================================================

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null)
            return;

        musicSource.Stop();

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }


    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }


    // =====================================================
    // GENERAL SFX
    // =====================================================

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip);
    }


    // =====================================================
    // PLAYER
    // =====================================================

    public void PlayFootstep()
    {
        if (footstepSounds == null ||
            footstepSounds.Length == 0)
            return;

        AudioClip clip =
            footstepSounds[
                Random.Range(
                    0,
                    footstepSounds.Length
                )
            ];

        if (playerSource != null)
        {
            playerSource.PlayOneShot(clip);
        }
    }


    public void PlaySwordSlash()
    {
        PlayPlayerSound(swordSlash);
    }


    public void PlayGunShot()
    {
        PlayPlayerSound(gunShot);
    }


    public void PlayReload()
    {
        PlayPlayerSound(reload);
    }


    public void PlayWeaponSwitch()
    {
        PlayPlayerSound(weaponSwitch);
    }


    public void PlayHeal()
    {
        PlayPlayerSound(heal);
    }


    public void PlayPickup()
    {
        PlayPlayerSound(pickup);
    }


    void PlayPlayerSound(AudioClip clip)
    {
        if (clip == null ||
            playerSource == null)
            return;

        playerSource.PlayOneShot(clip);
    }


    // =====================================================
    // PLAYER BREATHING
    // =====================================================

    public void PlayBreathing()
    {
        if (breathing == null ||
            playerSource == null)
            return;

        playerSource.clip = breathing;
        playerSource.loop = true;
        playerSource.Play();
    }


    public void StopBreathing()
    {
        if (playerSource == null)
            return;

        playerSource.Stop();
        playerSource.clip = null;
    }


    // =====================================================
    // ZOMBIE
    // =====================================================

    public void PlayZombieIdle()
    {
        PlayZombieSound(zombieIdle);
    }


    public void PlayZombieAttack()
    {
        PlayZombieSound(zombieAttack);
    }


    public void PlayZombieHit()
    {
        PlayZombieSound(zombieHit);
    }


    public void PlayZombieDeath()
    {
        PlayZombieSound(zombieDeath);
    }


    public void PlayZombieFootstep()
    {
        if (zombieFootsteps == null ||
            zombieFootsteps.Length == 0)
            return;

        AudioClip clip =
            zombieFootsteps[
                Random.Range(
                    0,
                    zombieFootsteps.Length
                )
            ];

        PlayZombieSound(clip);
    }


    void PlayZombieSound(AudioClip clip)
    {
        if (clip == null ||
            zombieSource == null)
            return;

        zombieSource.PlayOneShot(clip);
    }


    // =====================================================
    // BOSS AUDIO CONTROL
    // =====================================================

    public void EnableBossAudio()
    {
        bossAudioEnabled = true;

        Debug.Log(
            "=== BOSS AUDIO ENABLED ==="
        );
    }


    public void DisableBossAudio()
    {
        bossAudioEnabled = false;

        if (bossSource != null)
        {
            bossSource.Stop();
        }

        Debug.Log(
            "=== BOSS AUDIO DISABLED ==="
        );
    }


    // =====================================================
    // BOSS CUTSCENE ROAR
    // =====================================================

    // Hàm này được gọi khi Player bước vào Trigger.
    // Không cần EnableBossAudio().
    // Đây là tiếng gào riêng cho Cutscene.

    public void PlayBossCutsceneRoar(
        AudioClip cutsceneRoar,
        float volume = 1f)
    {
        if (cutsceneRoar == null)
        {
            Debug.LogWarning(
                "Chưa gán Boss Cutscene Roar!"
            );

            return;
        }

        if (bossSource == null)
        {
            Debug.LogWarning(
                "Boss Source chưa được gán!"
            );

            return;
        }

        bossSource.PlayOneShot(
            cutsceneRoar,
            volume
        );

        Debug.Log(
            "=== BOSS CUTSCENE ROAR ==="
        );
    }


    // =====================================================
    // BOSS ROAR
    // =====================================================

    public void PlayBossRoar()
    {
        if (!bossAudioEnabled)
            return;

        PlayBossSound(bossRoar);
    }


    // =====================================================
    // BOSS ATTACK
    // =====================================================

    public void PlayBossAttack()
    {
        if (!bossAudioEnabled)
            return;

        PlayBossSound(bossAttack);
    }


    // =====================================================
    // BOSS HIT
    // =====================================================

    public void PlayBossHit()
    {
        if (!bossAudioEnabled)
            return;

        PlayBossSound(bossHit);
    }


    // =====================================================
    // BOSS DEATH
    // =====================================================

    public void PlayBossDeath()
    {
        if (!bossAudioEnabled)
            return;

        PlayBossSound(bossDeath);
    }


    // =====================================================
    // BOSS FOOTSTEP
    // =====================================================

    public void PlayBossFootstep()
    {
        if (!bossAudioEnabled)
            return;

        if (bossFootsteps == null ||
            bossFootsteps.Length == 0)
            return;

        AudioClip clip =
            bossFootsteps[
                Random.Range(
                    0,
                    bossFootsteps.Length
                )
            ];

        PlayBossSound(clip);
    }


    // =====================================================
    // PLAY BOSS SOUND
    // =====================================================

    void PlayBossSound(AudioClip clip)
    {
        if (!bossAudioEnabled)
            return;

        if (clip == null ||
            bossSource == null)
            return;

        bossSource.PlayOneShot(clip);
    }


    // =====================================================
    // BOSS MUSIC
    // =====================================================

    public void PlayBossMusic()
    {
        if (bossFightMusic == null ||
            musicSource == null)
            return;

        musicSource.Stop();

        musicSource.clip = bossFightMusic;
        musicSource.loop = true;

        musicSource.Play();

        Debug.Log(
            "=== BOSS FIGHT MUSIC ON ==="
        );
    }
}