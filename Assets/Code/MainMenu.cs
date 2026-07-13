using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Chơi mới
    public void NewGame()
    {
        PlayerPrefs.DeleteKey("SaveScene");
        PlayerPrefs.DeleteKey("PlayerX");
        PlayerPrefs.DeleteKey("PlayerY");
        PlayerPrefs.DeleteKey("PlayerZ");

        SceneManager.LoadScene("GamePlay");
    }

    // Chơi tiếp
    public void ContinueGame()
    {
        if (!PlayerPrefs.HasKey("SaveScene"))
        {
            // Chưa có save thì coi như chơi mới
            SceneManager.LoadScene("GamePlay");
            return;
        }

        SceneManager.LoadScene(PlayerPrefs.GetString("SaveScene"));
    }

    // Thoát game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}