using UnityEngine;
using TMPro;

public class DisplaySettings : MonoBehaviour
{
    [Header("Window Mode")]
    public TMP_Dropdown windowDropdown;

    private const string WINDOW_MODE_KEY = "WindowMode";

    void Start()
    {
        SetupDropdown();
        LoadWindowMode();
    }

    // =====================================================
    // TẠO DANH SÁCH DROPDOWN
    // =====================================================

    void SetupDropdown()
    {
        windowDropdown.ClearOptions();

        windowDropdown.AddOptions(
            new System.Collections.Generic.List<string>
            {
                "Toàn màn hình",
                "Không viền",
                "Cửa sổ"
            }
        );
    }

    // =====================================================
    // KHI NGƯỜI CHƠI CHỌN
    // =====================================================

    public void ChangeWindowMode(int mode)
    {
        switch (mode)
        {
            // 0 = TOÀN MÀN HÌNH
            case 0:

                Screen.fullScreenMode =
                    FullScreenMode.ExclusiveFullScreen;

                Screen.fullScreen = true;

                break;


            // 1 = KHÔNG VIỀN
            case 1:

                Screen.fullScreenMode =
                    FullScreenMode.FullScreenWindow;

                Screen.fullScreen = true;

                break;


            // 2 = CỬA SỔ
            case 2:

                Screen.fullScreenMode =
                    FullScreenMode.Windowed;

                Screen.fullScreen = false;

                break;
        }

        PlayerPrefs.SetInt(
            WINDOW_MODE_KEY,
            mode
        );

        PlayerPrefs.Save();

        Debug.Log("Window Mode: " + mode);
    }

    // =====================================================
    // LOAD SETTING
    // =====================================================

    void LoadWindowMode()
    {
        int savedMode =
            PlayerPrefs.GetInt(
                WINDOW_MODE_KEY,
                0
            );

        windowDropdown.value = savedMode;
        windowDropdown.RefreshShownValue();

        ChangeWindowMode(savedMode);
    }
}