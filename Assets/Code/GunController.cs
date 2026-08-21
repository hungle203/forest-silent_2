using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GunController : MonoBehaviour
{
    [Header("Shoot")]
    public Camera playerCamera;
    public Transform firePoint;

    public ParticleSystem muzzleFlash;
    public GameObject bulletPrefab;

    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 0.1f;

    float nextFireTime;

    [Header("Gun Sound")]
    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    [Range(0f, 1f)]
    public float shootVolume = 1f;

    [Range(0f, 1f)]
    public float reloadVolume = 1f;

    [Header("Ammo")]
    public int magazineSize = 69;
    public int currentAmmo = 69;
    public int reserveAmmo = 69;
    public float reloadTime = 2f;

    bool isReloading;

    [Header("Recoil")]
    public Transform gunHolder;
    public Vector3 recoilKick = new Vector3(0, 0, -0.08f);
    public float recoilSpeed = 20f;
    public float returnSpeed = 8f;

    Vector3 originalPos;
    Vector3 targetPos;

    void Start()
    {
        originalPos = gunHolder.localPosition;
        targetPos = originalPos;

        // Nếu chưa kéo AudioSource vào Inspector
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        // =========================
        // RECOIL
        // =========================

        gunHolder.localPosition = Vector3.Lerp(
            gunHolder.localPosition,
            targetPos,
            recoilSpeed * Time.deltaTime
        );

        targetPos = Vector3.Lerp(
            targetPos,
            originalPos,
            returnSpeed * Time.deltaTime
        );

        if (isReloading)
            return;

        // =========================
        // RELOAD BẰNG R
        // =========================

        if (Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame)
        {
            StartCoroutine(Reload());
        }

        // =========================
        // BẮN
        // =========================

        if (Mouse.current != null &&
            Mouse.current.leftButton.isPressed &&
            Time.time >= nextFireTime &&
            currentAmmo > 0)
        {
            nextFireTime = Time.time + fireRate;

            currentAmmo--;

            Shoot();
        }

        // =========================
        // TỰ RELOAD
        // =========================

        if (currentAmmo <= 0 &&
            reserveAmmo > 0 &&
            !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    // =====================================================
    // SHOOT
    // =====================================================

    void Shoot()
    {
        // =========================
        // ÂM THANH BẮN
        // =========================

        if (audioSource != null &&
            shootSound != null)
        {
            audioSource.PlayOneShot(
                shootSound,
                shootVolume
            );
        }

        // =========================
        // MUZZLE FLASH
        // =========================

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // =========================
        // RECOIL
        // =========================

        targetPos =
            originalPos + recoilKick;

        // =========================
        // RAYCAST
        // =========================

        Ray ray =
            playerCamera.ViewportPointToRay(
                new Vector3(0.5f, 0.5f)
            );

        Vector3 hitPoint;

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            range))
        {
            hitPoint = hit.point;

            Debug.Log(hit.collider.name);

            // Nếu bắn Zombie
            ZombieAI zombie =
                hit.collider.GetComponentInParent<ZombieAI>();

            if (zombie != null)
            {
                zombie.TakeDamage(damage);

                zombie.SpawnBlood(
                    hit.point,
                    hit.normal
                );
            }
        }
        else
        {
            hitPoint =
                ray.origin +
                ray.direction * range;
        }

        // =========================
        // SPAWN BULLET
        // =========================

        Vector3 dir =
            (hitPoint - firePoint.position)
            .normalized;

        if (bulletPrefab != null)
        {
            GameObject bullet =
                Instantiate(
                    bulletPrefab,
                    firePoint.position,
                    Quaternion.LookRotation(dir)
                );

            // Nếu model đạn nằm theo trục Y
            bullet.transform.Rotate(
                -90,
                0,
                0
            );
        }
    }

    // =====================================================
    // RELOAD
    // =====================================================

    IEnumerator Reload()
    {
        if (isReloading)
            yield break;

        if (currentAmmo == magazineSize)
            yield break;

        if (reserveAmmo <= 0)
            yield break;

        isReloading = true;

        Debug.Log("Reloading...");

        // =========================
        // ÂM THANH NẠP ĐẠN
        // =========================

        if (audioSource != null &&
            reloadSound != null)
        {
            audioSource.PlayOneShot(
                reloadSound,
                reloadVolume
            );
        }

        // =========================
        // CHỜ RELOAD
        // =========================

        yield return new WaitForSeconds(
            reloadTime
        );

        int needAmmo =
            magazineSize - currentAmmo;

        int ammoToLoad =
            Mathf.Min(
                needAmmo,
                reserveAmmo
            );

        currentAmmo += ammoToLoad;

        reserveAmmo -= ammoToLoad;

        isReloading = false;

        Debug.Log(
            "Reload complete: " +
            currentAmmo +
            "/" +
            reserveAmmo
        );
    }

    // =====================================================
    // ADD AMMO
    // =====================================================

    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;
    }
}