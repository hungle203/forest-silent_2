using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private const string GAMEPLAY_SCENE = "GamePlay";

    private void Start()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ==================================================
    // NEW GAME
    // ==================================================

    public void NewGame()
    {
        SaveGame.DeleteSave();

        Time.timeScale = 1f;

        SceneManager.LoadScene(GAMEPLAY_SCENE);
    }

    // ==================================================
    // CONTINUE
    // ==================================================

    public void ContinueGame()
    {
        Time.timeScale = 1f;

        if (!SaveGame.HasSave())
        {
            Debug.Log(
                "Không có Save -> bắt đầu Game mới."
            );

            SceneManager.LoadScene(
                GAMEPLAY_SCENE
            );

            return;
        }

        Debug.Log(
            "Có Save -> Continue Game."
        );

        SceneManager.LoadScene(
            GAMEPLAY_SCENE
        );
    }

    // ==================================================
    // QUIT
    // ==================================================

    public void QuitGame()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#else

        Application.Quit();

#endif
    }
}