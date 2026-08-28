using System.Collections;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public static SaveGame Instance { get; private set; }

    [Header("Player")]
    [SerializeField] private Transform player;

    // =====================================================
    // SAVE KEYS
    // =====================================================

    private const string HAS_SAVE = "HasSave";

    private const string PLAYER_X = "PlayerX";
    private const string PLAYER_Y = "PlayerY";
    private const string PLAYER_Z = "PlayerZ";

    private const string PLAYER_ROT_Y = "PlayerRotY";

    private const string PLAYER_HEALTH = "PlayerHealth";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(InitializeSave());
    }

    // =====================================================
    // INITIALIZE
    // =====================================================

    private IEnumerator InitializeSave()
    {
        // Chờ Player được tạo hoàn chỉnh
        yield return null;

        FindPlayer();

        // Nếu có Save → Load
        if (HasSave())
        {
            Load();
        }
    }

    // =====================================================
    // FIND PLAYER
    // =====================================================

    private bool FindPlayer()
    {
        if (player != null)
            return true;

        GameObject obj =
            GameObject.FindGameObjectWithTag("Player");

        if (obj != null)
        {
            player = obj.transform;
            return true;
        }

        Debug.LogWarning(
            "SaveGame: Không tìm thấy Player! " +
            "Hãy kiểm tra Tag = Player."
        );

        return false;
    }

    // =====================================================
    // SAVE
    // =====================================================

    public bool Save()
    {
        if (!FindPlayer())
            return false;

        // -------------------------
        // PLAYER POSITION
        // -------------------------

        Vector3 pos = player.position;

        PlayerPrefs.SetFloat(PLAYER_X, pos.x);
        PlayerPrefs.SetFloat(PLAYER_Y, pos.y);
        PlayerPrefs.SetFloat(PLAYER_Z, pos.z);

        // -------------------------
        // PLAYER ROTATION
        // -------------------------

        PlayerPrefs.SetFloat(
            PLAYER_ROT_Y,
            player.eulerAngles.y
        );

        // -------------------------
        // PLAYER HEALTH
        // -------------------------

        PlayerHealth health =
            player.GetComponent<PlayerHealth>();

        if (health != null)
        {
            PlayerPrefs.SetFloat(
                PLAYER_HEALTH,
                health.GetHealth()
            );
        }

        // -------------------------
        // HAS SAVE
        // -------------------------

        PlayerPrefs.SetInt(HAS_SAVE, 1);

        // Ghi xuống ổ đĩa
        PlayerPrefs.Save();

        Debug.Log(
            "========== GAME SAVED =========="
        );

        Debug.Log(
            "Position: " + pos
        );

        if (health != null)
        {
            Debug.Log(
                "Health: " + health.GetHealth()
            );
        }

        Debug.Log(
            "================================"
        );

        return true;
    }

    // =====================================================
    // LOAD
    // =====================================================

    public void Load()
    {
        if (!HasSave())
        {
            Debug.Log(
                "SaveGame: Không có Save."
            );

            return;
        }

        if (!FindPlayer())
            return;

        // -------------------------
        // POSITION
        // -------------------------

        Vector3 savedPosition =
            new Vector3(
                PlayerPrefs.GetFloat(PLAYER_X),
                PlayerPrefs.GetFloat(PLAYER_Y),
                PlayerPrefs.GetFloat(PLAYER_Z)
            );

        // CharacterController
        CharacterController controller =
            player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        player.position = savedPosition;

        // -------------------------
        // ROTATION
        // -------------------------

        float rotationY =
            PlayerPrefs.GetFloat(
                PLAYER_ROT_Y,
                player.eulerAngles.y
            );

        player.rotation =
            Quaternion.Euler(
                0f,
                rotationY,
                0f
            );

        if (controller != null)
            controller.enabled = true;

        // -------------------------
        // HEALTH
        // -------------------------

        PlayerHealth health =
            player.GetComponent<PlayerHealth>();

        if (health != null &&
            PlayerPrefs.HasKey(PLAYER_HEALTH))
        {
            float savedHealth =
                PlayerPrefs.GetFloat(
                    PLAYER_HEALTH
                );

            health.SetHealth(savedHealth);
        }

        Debug.Log(
            "========== GAME LOADED =========="
        );

        Debug.Log(
            "Position: " + savedPosition
        );

        if (health != null)
        {
            Debug.Log(
                "Health: " +
                health.GetHealth()
            );
        }

        Debug.Log(
            "================================="
        );
    }

    // =====================================================
    // HAS SAVE
    // =====================================================

    public static bool HasSave()
    {
        return PlayerPrefs.GetInt(
            HAS_SAVE,
            0
        ) == 1;
    }

    // =====================================================
    // DELETE SAVE
    // =====================================================

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(HAS_SAVE);

        PlayerPrefs.DeleteKey(PLAYER_X);
        PlayerPrefs.DeleteKey(PLAYER_Y);
        PlayerPrefs.DeleteKey(PLAYER_Z);

        PlayerPrefs.DeleteKey(PLAYER_ROT_Y);

        PlayerPrefs.DeleteKey(PLAYER_HEALTH);

        PlayerPrefs.Save();

        Debug.Log(
            "========== SAVE DELETED =========="
        );
    }
}