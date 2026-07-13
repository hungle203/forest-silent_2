using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGame : MonoBehaviour
{
    public Transform player;

    public void Save()
    {
        PlayerPrefs.SetString("SaveScene",
            SceneManager.GetActiveScene().name);

        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.position.z);

        PlayerPrefs.Save();
    }
}