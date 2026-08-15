using UnityEngine;
using UnityEngine.Playables;

public class BossCutscene : MonoBehaviour
{
    [Header("Timeline")]
    public PlayableDirector cameraTimeline;
    public PlayableDirector bossTimeline;

    [Header("Boss")]
    public BossAI bossAI;

    [Header("Player")]
    public MonoBehaviour playerController;

    [Header("Player Camera")]
    public GameObject playerCamera;

    [Header("Player UI")]
    public GameObject playerUI;

    private bool cutscenePlayed = false;

    void Start()
    {
        // Không cho Timeline tự chạy
        if (cameraTimeline != null)
        {
            cameraTimeline.playOnAwake = false;
            cameraTimeline.Stop();
        }

        if (bossTimeline != null)
        {
            bossTimeline.playOnAwake = false;
            bossTimeline.Stop();
        }

        // Boss chưa hoạt động
        if (bossAI != null)
        {
            bossAI.enabled = false;
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

        StartCutscene();
    }

    // =========================================================
    // START CUTSCENE
    // =========================================================

    public void StartCutscene()
    {
        Debug.Log("=== BOSS CUTSCENE START ===");

        // =========================
        // TẮT PLAYER
        // =========================

        if (playerController != null)
        {
            playerController.enabled = false;
            Debug.Log("Player Controller OFF");
        }

        // =========================
        // TẮT PLAYER CAMERA
        // =========================

        if (playerCamera != null)
        {
            playerCamera.SetActive(false);
            Debug.Log("Player Camera OFF");
        }

        // =========================
        // TẮT PLAYER UI
        // =========================

        if (playerUI != null)
        {
            playerUI.SetActive(false);
            Debug.Log("Player UI OFF");
        }

        // =========================
        // TẮT BOSS AI
        // =========================

        if (bossAI != null)
        {
            bossAI.enabled = false;
            Debug.Log("Boss AI OFF");
        }

        // =========================
        // CAMERA TIMELINE
        // =========================

        if (cameraTimeline != null)
        {
            Debug.Log("Camera Timeline PLAY");

            cameraTimeline.stopped -= OnCameraFinished;
            cameraTimeline.stopped += OnCameraFinished;

            cameraTimeline.Play();
        }
        else
        {
            Debug.LogError("Camera Timeline chưa được gán!");

            PlayBossTimeline();
        }
    }

    // =========================================================
    // CAMERA TIMELINE FINISHED
    // =========================================================

    private void OnCameraFinished(PlayableDirector director)
    {
        cameraTimeline.stopped -= OnCameraFinished;

        Debug.Log("Camera Timeline FINISHED");

        PlayBossTimeline();
    }

    // =========================================================
    // BOSS TIMELINE
    // =========================================================

    private void PlayBossTimeline()
    {
        if (bossTimeline != null)
        {
            Debug.Log("Boss Timeline PLAY");

            bossTimeline.stopped -= OnBossFinished;
            bossTimeline.stopped += OnBossFinished;

            bossTimeline.Play();
        }
        else
        {
            Debug.LogWarning("Không có Boss Timeline!");

            EndCutscene();
        }
    }

    // =========================================================
    // BOSS TIMELINE FINISHED
    // =========================================================

    private void OnBossFinished(PlayableDirector director)
    {
        bossTimeline.stopped -= OnBossFinished;

        Debug.Log("Boss Timeline FINISHED");

        EndCutscene();
    }

    // =========================================================
    // END CUTSCENE
    // =========================================================

    private void EndCutscene()
    {
        Debug.Log("=== CUTSCENE END ===");

        // =========================
        // BẬT PLAYER CAMERA
        // =========================

        if (playerCamera != null)
        {
            playerCamera.SetActive(true);
            Debug.Log("Player Camera ON");
        }

        // =========================
        // BẬT PLAYER UI
        // =========================

        if (playerUI != null)
        {
            playerUI.SetActive(true);
            Debug.Log("Player UI ON");
        }

        // =========================
        // BẬT PLAYER CONTROLLER
        // =========================

        if (playerController != null)
        {
            playerController.enabled = true;
            Debug.Log("Player Controller ON");
        }

        // =========================
        // BẬT BOSS AI
        // =========================

        if (bossAI != null)
        {
            bossAI.enabled = true;
            Debug.Log("Boss AI ON");
        }

        Debug.Log("BOSS BẮT ĐẦU ĐUỔI VÀ TẤN CÔNG PLAYER!");
    }
}