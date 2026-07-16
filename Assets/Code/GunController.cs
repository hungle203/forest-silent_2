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

    [Header("Ammo")]
    public int magazineSize = 69;      // sức chứa băng
    public int currentAmmo = 69;       // đạn trong băng
    public int reserveAmmo = 69;       // đạn dự trữ
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
    }

    void Update()
    {
        // Recoil
        gunHolder.localPosition = Vector3.Lerp(
            gunHolder.localPosition,
            targetPos,
            recoilSpeed * Time.deltaTime);

        targetPos = Vector3.Lerp(
            targetPos,
            originalPos,
            returnSpeed * Time.deltaTime);

        if (isReloading)
            return;

        // Reload bằng phím R
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            StartCoroutine(Reload());
        }

        // Bắn
        if (Mouse.current.leftButton.isPressed &&
            Time.time >= nextFireTime &&
            currentAmmo > 0)
        {
            nextFireTime = Time.time + fireRate;

            currentAmmo--;

            Shoot();
        }

        // Tự reload khi hết đạn
        if (currentAmmo <= 0 &&
            reserveAmmo > 0 &&
            !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        // Recoil
        targetPos = originalPos + recoilKick;

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f));

        Vector3 hitPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            hitPoint = hit.point;
            Debug.Log(hit.collider.name);
        }
        else
        {
            hitPoint = ray.origin + ray.direction * range;
        }

        // Spawn viên đạn
        Vector3 dir = (hitPoint - firePoint.position).normalized;

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(dir));

        // Nếu model đạn nằm theo trục Y
        bullet.transform.Rotate(-90, 0, 0);
    }

    IEnumerator Reload()
    {
        if (currentAmmo == magazineSize)
            yield break;

        if (reserveAmmo <= 0)
            yield break;

        isReloading = true;

        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        int needAmmo = magazineSize - currentAmmo;

        int ammoToLoad = Mathf.Min(needAmmo, reserveAmmo);

        currentAmmo += ammoToLoad;

        reserveAmmo -= ammoToLoad;

        isReloading = false;
    }

    // Hàm dùng khi nhặt hộp đạn
    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;
    }
}