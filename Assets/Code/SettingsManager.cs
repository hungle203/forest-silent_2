using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource[] sfxSources;

    private const string BGM_VOLUME = "BGMVolume";
    private const string SFX_VOLUME = "SFXVolume";
    private const string FULLSCREEN = "Fullscreen";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadSettings();
    }

    // =========================
    // BGM
    // =========================

    public void SetBGMVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        if (musicSource != null)
            musicSource.volume = volume;

        PlayerPrefs.SetFloat(BGM_VOLUME, volume);
        PlayerPrefs.Save();
    }

    // =========================
    // SFX
    // =========================

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        foreach (AudioSource source in sfxSources)
        {
            if (source != null)
                source.volume = volume;
        }

        PlayerPrefs.SetFloat(SFX_VOLUME, volume);
        PlayerPrefs.Save();
    }

    // =========================
    // FULLSCREEN
    // =========================

    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;

        PlayerPrefs.SetInt(FULLSCREEN, fullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    // =========================
    // LOAD SETTINGS
    // =========================

    private void LoadSettings()
    {
        float bgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME, 1f);

        bool fullscreen =
            PlayerPrefs.GetInt(FULLSCREEN, 1) == 1;

        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);

        Screen.fullScreen = fullscreen;
    }
}