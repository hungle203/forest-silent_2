using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause UI")]
    public GameObject pause;

    private bool isPaused;

    private void Start()
    {
        isPaused = false;

        if (pause != null)
            pause.SetActive(false);

        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        isPaused = true;

        if (pause != null)
            pause.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (pause != null)
            pause.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // =====================================================
    // EXIT TO MAIN MENU
    // =====================================================

    public void ExitToMainMenu()
    {
        // SAVE TRƯỚC
        if (SaveGame.Instance != null)
        {
            bool saved =
                SaveGame.Instance.Save();

            if (saved)
            {
                Debug.Log(
                    "Đã Save trước khi về Main Menu."
                );
            }
            else
            {
                Debug.LogWarning(
                    "Save thất bại!"
                );
            }
        }
        else
        {
            Debug.LogError(
                "Không tìm thấy SaveGame.Instance!"
            );
        }

        // Reset game time
        Time.timeScale = 1f;

        // Hiện chuột
        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        // Về Main Menu
        SceneManager.LoadScene(
            "MainMenu"
        );
    }
}