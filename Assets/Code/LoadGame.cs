using UnityEngine;

public class LoadGame : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
            return;

        player.position = new Vector3(
            PlayerPrefs.GetFloat("PlayerX"),
            PlayerPrefs.GetFloat("PlayerY"),
            PlayerPrefs.GetFloat("PlayerZ"));
    }
}