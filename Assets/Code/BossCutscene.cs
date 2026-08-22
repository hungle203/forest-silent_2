using UnityEngine;
using UnityEngine.Playables;

public class BossCutscene : MonoBehaviour
{
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
        Debug.Log("=================================");
        Debug.Log("=== BOSS CUTSCENE SYSTEM READY ===");
        Debug.Log("=================================");


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
        // BOSS GAMEPLAY OFF
        // =====================================================

        if (bossAI != null)
        {
            bossAI.SetGameplayEnabled(false);

            Debug.Log(
                "Boss Gameplay OFF - waiting for trigger"
            );
        }


        // =====================================================
        // BOSS AUDIO READY
        // =====================================================

        if (bossAI != null)
        {
            bossAI.EnableBossAudio();

            Debug.Log(
                "Boss Audio READY"
            );
        }
    }


    // =========================================================
    // TRIGGER
    // =========================================================

    private void OnTriggerEnter(Collider other)
    {
        if (cutscenePlayed)
            return;

        if (!other.CompareTag("Player"))
            return;

        cutscenePlayed = true;

        Debug.Log("=================================");
        Debug.Log("PLAYER ĐÃ VÀO BOSS TRIGGER!");
        Debug.Log("=================================");

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

        Debug.Log("=================================");
        Debug.Log("=== BOSS CUTSCENE START ===");
        Debug.Log("=================================");


        // =====================================================
        // GAMEPLAY MUSIC OFF
        // =====================================================

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();

            Debug.Log(
                "Gameplay BGM OFF"
            );
        }


        // =====================================================
        // BOSS AUDIO ON
        // =====================================================

        if (bossAI != null)
        {
            bossAI.EnableBossAudio();

            Debug.Log(
                "Boss Audio READY"
            );
        }


        // =====================================================
        // BOSS GAMEPLAY OFF
        // =====================================================

        if (bossAI != null)
        {
            bossAI.SetGameplayEnabled(false);

            Debug.Log(
                "Boss Gameplay OFF - Cutscene"
            );
        }


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
    // CAMERA TIMELINE FINISHED
    // =========================================================

    private void OnCameraFinished(
        PlayableDirector director
    )
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
    // BOSS ROAR FROM TIMELINE
    // =========================================================

    public void PlayBossRoarFromTimeline()
    {
        if (bossAI == null)
        {
            Debug.LogWarning(
                "BossAI chưa được gán!"
            );

            return;
        }

        Debug.Log(
            "================================="
        );

        Debug.Log(
            "=== TIMELINE: BOSS ROAR ==="
        );

        Debug.Log(
            "================================="
        );

        bossAI.PlayRoarFromCutscene();
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
        PlayableDirector director
    )
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
        Debug.Log("=================================");
        Debug.Log("=== CUTSCENE END ===");
        Debug.Log("=================================");


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
        // BOSS MUSIC
        // =====================================================

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBossMusic();

            Debug.Log(
                "=== BOSS FIGHT MUSIC ON ==="
            );
        }


        // =====================================================
        // BOSS AUDIO
        // =====================================================

        if (bossAI != null)
        {
            bossAI.EnableBossAudio();
        }


        // =====================================================
        // BOSS GAMEPLAY ON
        // =====================================================

        if (bossAI != null)
        {
            bossAI.SetGameplayEnabled(true);

            Debug.Log(
                "=== BOSS GAMEPLAY ON ==="
            );

            Debug.Log(
                "=== BOSS BẮT ĐẦU CHASE PLAYER ==="
            );

            Debug.Log(
                "=== BOSS AUDIO ĐANG HOẠT ĐỘNG ==="
            );
        }


        cutsceneRunning = false;


        // =====================================================
        // DISABLE CUTSCENE OBJECT
        // =====================================================

        Debug.Log(
            "=== BOSS CUTSCENE OBJECT OFF ==="
        );

        gameObject.SetActive(false);
    }
}