using UnityEngine;
using UnityEngine.Playables;

public class BossCutscene : MonoBehaviour
{
    [Header("Boss Music")]
    public AudioClip bossFightMusic;

    // =========================================================
    // AUDIO LISTENER
    // =========================================================

    [Header("Audio Listener")]

    public AudioListener playerAudioListener;
    public AudioListener cutsceneAudioListener;


    // =========================================================
    // CUTSCENE BOSS AUDIO
    // =========================================================

    [Header("Boss Cutscene Audio")]

    public AudioSource bossAudioSource;

    [Tooltip("Tiếng Boss gào khi bắt đầu cutscene")]
    public AudioClip bossRoarSound;

    [Range(0f, 1f)]
    public float bossRoarVolume = 1f;


    // =========================================================
    // TIMELINE
    // =========================================================

    [Header("Timeline")]

    public PlayableDirector cameraTimeline;
    public PlayableDirector bossTimeline;


    // =========================================================
    // BOSS
    // =========================================================

    [Header("Boss")]

    public BossAI bossAI;


    // =========================================================
    // PLAYER
    // =========================================================

    [Header("Player")]

    public MonoBehaviour playerController;


    // =========================================================
    // PLAYER CAMERA
    // =========================================================

    [Header("Player Camera")]

    public GameObject playerCamera;


    // =========================================================
    // PLAYER UI
    // =========================================================

    [Header("Player UI")]

    public GameObject playerUI;


    // =========================================================
    // CUTSCENE CAMERA
    // =========================================================

    [Header("Cutscene Camera")]

    public GameObject cutsceneCamera;


    // =========================================================
    // STATE
    // =========================================================

    private bool cutscenePlayed = false;
    private bool cutsceneRunning = false;


    // =========================================================
    // START
    // =========================================================

    void Start()
    {
        Debug.Log("=== BOSS CUTSCENE SYSTEM READY ===");


        // =====================================================
        // AUDIO LISTENER BAN ĐẦU
        // =====================================================

        if (playerAudioListener != null)
        {
            playerAudioListener.enabled = true;
        }

        if (cutsceneAudioListener != null)
        {
            cutsceneAudioListener.enabled = false;
        }


        // =====================================================
        // CUTSCENE CAMERA OFF
        // =====================================================

        if (cutsceneCamera != null)
        {
            cutsceneCamera.SetActive(false);
        }


        // =====================================================
        // PLAYER CAMERA ON
        // =====================================================

        if (playerCamera != null)
        {
            playerCamera.SetActive(true);
        }


        // =====================================================
        // PLAYER UI ON
        // =====================================================

        if (playerUI != null)
        {
            playerUI.SetActive(true);
        }


        // =====================================================
        // CAMERA TIMELINE OFF
        // =====================================================

        if (cameraTimeline != null)
        {
            cameraTimeline.playOnAwake = false;
            cameraTimeline.Stop();
        }


        // =====================================================
        // BOSS TIMELINE OFF
        // =====================================================

        if (bossTimeline != null)
        {
            bossTimeline.playOnAwake = false;
            bossTimeline.Stop();
        }


        // =====================================================
        // BOSS AI OFF
        // =====================================================

        if (bossAI != null)
        {
            bossAI.enabled = false;

            // QUAN TRỌNG:
            // Boss không được phát âm thanh khi game mới bắt đầu.

            bossAI.DisableBossAudio();

            Debug.Log("Boss AI OFF");
            Debug.Log("Boss Audio OFF");
        }


        // =====================================================
        // BOSS CUTSCENE AUDIO SOURCE
        // =====================================================

        if (bossAudioSource == null)
        {
            bossAudioSource =
                GetComponent<AudioSource>();
        }

        if (bossAudioSource != null)
        {
            bossAudioSource.playOnAwake = false;
            bossAudioSource.loop = false;

            bossAudioSource.Stop();

            Debug.Log(
                "Boss Cutscene Audio OFF - waiting for trigger"
            );
        }
    }


    // =========================================================
    // TRIGGER CUTSCENE
    // =========================================================

    private void OnTriggerEnter(Collider other)
    {
        if (cutscenePlayed)
            return;

        if (!other.CompareTag("Player"))
            return;

        cutscenePlayed = true;

        Debug.Log(
            "================================="
        );

        Debug.Log(
            "PLAYER ĐÃ VÀO BOSS TRIGGER!"
        );

        Debug.Log(
            "================================="
        );

        StartCutscene();
    }


    // =========================================================
    // START CUTSCENE
    // =========================================================

    public void StartCutscene()
    {
        if (cutsceneRunning)
            return;

        cutsceneRunning = true;


        Debug.Log(
            "================================="
        );

        Debug.Log(
            "=== BOSS CUTSCENE START ==="
        );

        Debug.Log(
            "================================="
        );


        // =====================================================
        // TẮT GAMEPLAY MUSIC
        // =====================================================

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();

            Debug.Log(
                "Gameplay BGM OFF"
            );
        }


        // =====================================================
        // BOSS ROAR
        // =====================================================

        // Tiếng gào này CHỈ chạy khi Player trigger.

        PlayBossRoar();


        // =====================================================
        // PLAYER CONTROLLER OFF
        // =====================================================

        if (playerController != null)
        {
            playerController.enabled = false;

            Debug.Log(
                "Player Controller OFF"
            );
        }


        // =====================================================
        // PLAYER AUDIO LISTENER OFF
        // =====================================================

        if (playerAudioListener != null)
        {
            playerAudioListener.enabled = false;

            Debug.Log(
                "Player Audio Listener OFF"
            );
        }


        // =====================================================
        // CUTSCENE AUDIO LISTENER ON
        // =====================================================

        if (cutsceneAudioListener != null)
        {
            cutsceneAudioListener.enabled = true;

            Debug.Log(
                "Cutscene Audio Listener ON"
            );
        }


        // =====================================================
        // PLAYER CAMERA OFF
        // =====================================================

        if (playerCamera != null)
        {
            playerCamera.SetActive(false);

            Debug.Log(
                "Player Camera OFF"
            );
        }


        // =====================================================
        // PLAYER UI OFF
        // =====================================================

        if (playerUI != null)
        {
            playerUI.SetActive(false);

            Debug.Log(
                "Player UI OFF"
            );
        }


        // =====================================================
        // BOSS AI OFF
        // =====================================================

        if (bossAI != null)
        {
            bossAI.DisableBossAudio();

            bossAI.enabled = false;

            Debug.Log(
                "Boss AI OFF"
            );

            Debug.Log(
                "Boss SFX OFF during cutscene"
            );
        }


        // =====================================================
        // CUTSCENE CAMERA ON
        // =====================================================

        if (cutsceneCamera != null)
        {
            cutsceneCamera.SetActive(true);

            Debug.Log(
                "Cutscene Camera ON"
            );
        }
        else
        {
            Debug.LogWarning(
                "Cutscene Camera chưa được gán!"
            );
        }


        // =====================================================
        // CAMERA TIMELINE
        // =====================================================

        if (cameraTimeline != null)
        {
            Debug.Log(
                "Camera Timeline PLAY"
            );

            cameraTimeline.stopped -=
                OnCameraFinished;

            cameraTimeline.stopped +=
                OnCameraFinished;

            cameraTimeline.Play();
        }
        else
        {
            Debug.LogWarning(
                "Camera Timeline chưa được gán!"
            );

            PlayBossTimeline();
        }
    }


    // =========================================================
    // BOSS ROAR - CUTSCENE
    // =========================================================

    private void PlayBossRoar()
    {
        if (bossAudioSource == null)
        {
            Debug.LogWarning(
                "Boss Cutscene AudioSource chưa được gán!"
            );

            return;
        }

        if (bossRoarSound == null)
        {
            Debug.LogWarning(
                "Boss Roar Sound chưa được gán!"
            );

            return;
        }


        bossAudioSource.PlayOneShot(
            bossRoarSound,
            bossRoarVolume
        );


        Debug.Log(
            "=== BOSS CUTSCENE ROAR PLAY ==="
        );
    }


    // =========================================================
    // CAMERA TIMELINE FINISHED
    // =========================================================

    private void OnCameraFinished(
        PlayableDirector director)
    {
        if (cameraTimeline != null)
        {
            cameraTimeline.stopped -=
                OnCameraFinished;
        }


        Debug.Log(
            "Camera Timeline FINISHED"
        );


        PlayBossTimeline();
    }


    // =========================================================
    // BOSS TIMELINE
    // =========================================================

    private void PlayBossTimeline()
    {
        if (bossTimeline != null)
        {
            Debug.Log(
                "Boss Timeline PLAY"
            );

            bossTimeline.stopped -=
                OnBossFinished;

            bossTimeline.stopped +=
                OnBossFinished;

            bossTimeline.Play();
        }
        else
        {
            Debug.LogWarning(
                "Không có Boss Timeline!"
            );

            EndCutscene();
        }
    }


    // =========================================================
    // BOSS TIMELINE FINISHED
    // =========================================================

    private void OnBossFinished(
        PlayableDirector director)
    {
        if (bossTimeline != null)
        {
            bossTimeline.stopped -=
                OnBossFinished;
        }


        Debug.Log(
            "Boss Timeline FINISHED"
        );


        EndCutscene();
    }


    // =========================================================
    // END CUTSCENE
    // =========================================================

    private void EndCutscene()
    {
        Debug.Log(
            "================================="
        );

        Debug.Log(
            "=== CUTSCENE END ==="
        );

        Debug.Log(
            "================================="
        );


        // =====================================================
        // CUTSCENE CAMERA OFF
        // =====================================================

        if (cutsceneCamera != null)
        {
            cutsceneCamera.SetActive(false);

            Debug.Log(
                "Cutscene Camera OFF"
            );
        }


        // =====================================================
        // CUTSCENE AUDIO LISTENER OFF
        // =====================================================

        if (cutsceneAudioListener != null)
        {
            cutsceneAudioListener.enabled = false;

            Debug.Log(
                "Cutscene Audio Listener OFF"
            );
        }


        // =====================================================
        // PLAYER AUDIO LISTENER ON
        // =====================================================

        if (playerAudioListener != null)
        {
            playerAudioListener.enabled = true;

            Debug.Log(
                "Player Audio Listener ON"
            );
        }


        // =====================================================
        // PLAYER CAMERA ON
        // =====================================================

        if (playerCamera != null)
        {
            playerCamera.SetActive(true);

            Debug.Log(
                "Player Camera ON"
            );
        }


        // =====================================================
        // PLAYER UI ON
        // =====================================================

        if (playerUI != null)
        {
            playerUI.SetActive(true);

            Debug.Log(
                "Player UI ON"
            );
        }


        // =====================================================
        // BOSS MUSIC ON
        // =====================================================

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBossMusic();

            Debug.Log(
                "=== BOSS FIGHT MUSIC ON ==="
            );
        }


        // =====================================================
        // PLAYER CONTROLLER ON
        // =====================================================

        if (playerController != null)
        {
            playerController.enabled = true;

            Debug.Log(
                "Player Controller ON"
            );
        }


        // =====================================================
        // BOSS AI ON
        // =====================================================

        if (bossAI != null)
        {
            bossAI.enabled = true;

            // =================================================
            // MỞ TOÀN BỘ BOSS AUDIO
            // =================================================

            bossAI.EnableBossAudio();

            Debug.Log(
                "Boss AI ON"
            );

            Debug.Log(
                "=== BOSS AUDIO ON ==="
            );
        }


        cutsceneRunning = false;


        Debug.Log(
            "================================="
        );

        Debug.Log(
            "BOSS BẮT ĐẦU ĐUỔI VÀ TẤN CÔNG PLAYER!"
        );

        Debug.Log(
            "================================="
        );



        // =====================================================
// XÓA BOSS CUTSCENE SAU KHI CHẠY XONG
// =====================================================

gameObject.SetActive(false);

Debug.Log("=== BOSS CUTSCENE OBJECT OFF ===");
    }
}