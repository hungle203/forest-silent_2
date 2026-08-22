using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawnManager : MonoBehaviour
{
    [Header("Item Prefabs")]
    public GameObject medkitPrefab;
    public GameObject ammoPrefab;
    public GameObject pinPrefab;

    [Header("Spawn Areas")]
    public Transform[] spawnPoints;

    [Header("Amount")]
    public int medkitCount = 10;
    public int ammoCount = 20;
    public int pinCount = 10;

    [Header("Spawn Area")]
    public float spawnRadius = 10f;

    [Header("Item Distance")]
    public float minDistance = 2f;

    [Header("NavMesh")]
    public float navMeshSearchDistance = 5f;

    [Header("Ground")]
    public float groundOffset = 0.02f;

    [Header("Ammo Rotation")]
    public Vector3 ammoRotation = new Vector3(90f, 0f, 0f);

    private List<Vector3> spawnedPositions =
        new List<Vector3>();


    void Start()
    {
        SpawnItems();
    }


    void SpawnItems()
    {
        // ==========================
        // MEDKIT
        // ==========================

        SpawnItem(
            medkitPrefab,
            medkitCount,
            false
        );


        // ==========================
        // AMMO
        // ==========================

        SpawnItem(
            ammoPrefab,
            ammoCount,
            true
        );


        // ==========================
        // PIN
        // ==========================

        SpawnItem(
            pinPrefab,
            pinCount,
            false
        );
    }


    void SpawnItem(
        GameObject prefab,
        int amount,
        bool isAmmo)
    {
        if (prefab == null)
        {
            Debug.LogWarning(
                "Chưa gán Prefab!"
            );

            return;
        }

        if (spawnPoints == null ||
            spawnPoints.Length == 0)
        {
            Debug.LogWarning(
                "Chưa gán Spawn Points!"
            );

            return;
        }

        int spawned = 0;

        // Thử nhiều lần để tìm đủ vị trí
        int maxAttempts = amount * 100;

        for (
            int attempt = 0;
            attempt < maxAttempts;
            attempt++)
        {
            if (spawned >= amount)
                break;


            // ==========================
            // CHỌN SPAWN POINT NGẪU NHIÊN
            // ==========================

            Transform point =
                spawnPoints[
                    Random.Range(
                        0,
                        spawnPoints.Length
                    )
                ];

            if (point == null)
                continue;


            // ==========================
            // RANDOM VỊ TRÍ XUNG QUANH
            // ==========================

            Vector2 randomCircle =
                Random.insideUnitCircle *
                spawnRadius;

            Vector3 randomPosition =
                point.position +
                new Vector3(
                    randomCircle.x,
                    0f,
                    randomCircle.y
                );


            // ==========================
            // TÌM NAVMESH
            // ==========================

            if (!NavMesh.SamplePosition(
                    randomPosition,
                    out NavMeshHit hit,
                    navMeshSearchDistance,
                    NavMesh.AllAreas))
            {
                continue;
            }

            Vector3 spawnPosition =
                hit.position;


            // ==========================
            // KIỂM TRA KHOẢNG CÁCH
            // ==========================

            bool tooClose = false;

            foreach (
                Vector3 oldPosition
                in spawnedPositions)
            {
                if (Vector3.Distance(
                        spawnPosition,
                        oldPosition
                    ) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
                continue;


            // ==========================
            // ROTATION
            // ==========================

            Quaternion rotation;

            if (isAmmo)
            {
                // Băng đạn nằm xuống
                rotation =
                    Quaternion.Euler(
                        ammoRotation
                    );
            }
            else
            {
                // Medkit / Pin xoay ngẫu nhiên
                rotation =
                    Quaternion.Euler(
                        0f,
                        Random.Range(
                            0f,
                            360f
                        ),
                        0f
                    );
            }


            // ==========================
            // SPAWN
            // ==========================

            GameObject item =
                Instantiate(
                    prefab,
                    spawnPosition,
                    rotation
                );


            // ==========================
            // ĐƯA ITEM XUỐNG MẶT ĐẤT
            // ==========================

            PlaceOnGround(item);


            spawnedPositions.Add(
                item.transform.position
            );

            spawned++;
        }


        Debug.Log(
            "Spawn " +
            prefab.name +
            ": " +
            spawned +
            "/" +
            amount
        );
    }


    // ==========================================
    // ĐẶT ITEM LÊN MẶT ĐẤT
    // ==========================================

    void PlaceOnGround(GameObject item)
    {
        Collider[] colliders =
            item.GetComponentsInChildren<Collider>();

        if (colliders.Length == 0)
        {
            item.transform.position +=
                Vector3.up * groundOffset;

            return;
        }


        Bounds bounds =
            colliders[0].bounds;

        for (
            int i = 1;
            i < colliders.Length;
            i++)
        {
            bounds.Encapsulate(
                colliders[i].bounds
            );
        }


        float bottom =
            bounds.min.y;

        float offset =
            item.transform.position.y -
            bottom +
            groundOffset;

        item.transform.position +=
            Vector3.up * offset;
    }


    // ==========================================
    // GIZMOS
    // ==========================================

    void OnDrawGizmosSelected()
    {
        if (spawnPoints == null)
            return;

        Gizmos.color =
            Color.yellow;

        foreach (
            Transform point
            in spawnPoints)
        {
            if (point == null)
                continue;

            Gizmos.DrawWireSphere(
                point.position,
                spawnRadius
            );
        }
    }
}