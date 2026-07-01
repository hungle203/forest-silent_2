using UnityEngine;

public class ZombieSpawnManager : MonoBehaviour
{
    [Header("Zombie")]
    public GameObject zombiePrefab;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Spawn")]
    public int zombieCount = 10;
    public float spawnRadius = 5f;

    void Start()
    {
        SpawnZombies();
    }

    void SpawnZombies()
    {
        for (int i = 0; i < zombieCount; i++)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Lấy vị trí ngẫu nhiên trong bán kính
            Vector2 randomPos = Random.insideUnitCircle * spawnRadius;

            Vector3 spawnPosition = point.position +
                                    new Vector3(randomPos.x, 0f, randomPos.y);

            Instantiate(
                zombiePrefab,
                spawnPosition,
                point.rotation);
        }
    }

    void OnDrawGizmos()
    {
        if (spawnPoints == null)
            return;

        foreach (Transform point in spawnPoints)
        {
            if (point == null)
                continue;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(point.position, spawnRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(point.position, 0.2f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(point.position, point.position + point.forward * 2f);
        }
    }
}