using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet")]
    public float speed = 200f;
    public float damage = 25f;
    public float lifeTime = 3f;


    void Start()
    {
        Destroy(gameObject, lifeTime);
    }


    void Update()
    {
        transform.position +=
            transform.forward *
            speed *
            Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.name);


        // =====================================================
        // ZOMBIE
        // =====================================================

        ZombieAI zombie =
            other.GetComponentInParent<ZombieAI>();

        if (zombie != null)
        {
            Debug.Log(
                "BULLET HIT ZOMBIE: " +
                zombie.name
            );


            // Gây damage
            zombie.TakeDamage(damage);


            // Vị trí va chạm
            Vector3 hitPoint =
                other.ClosestPoint(
                    transform.position
                );


            // Hướng Blood
            Vector3 hitNormal =
                (hitPoint -
                 zombie.transform.position)
                .normalized;


            // Blood
            zombie.SpawnBlood(
                hitPoint,
                hitNormal
            );


            // Hủy Bullet
            Destroy(gameObject);

            return;
        }


        // =====================================================
        // BOSS
        // =====================================================

        BossAI boss =
            other.GetComponentInParent<BossAI>();

        if (boss != null)
        {
            Debug.Log(
                "================================="
            );

            Debug.Log(
                "BULLET HIT BOSS!"
            );

            Debug.Log(
                "Boss: " +
                boss.name
            );

            Debug.Log(
                "Damage: " +
                damage
            );

            Debug.Log(
                "================================="
            );


            // =================================================
            // GÂY DAMAGE CHO BOSS
            // =================================================

            boss.TakeDamage(damage);


            // =================================================
            // HỦY BULLET
            // =================================================

            Destroy(gameObject);

            return;
        }


        // =====================================================
        // OBJECT KHÁC
        // =====================================================

        Destroy(gameObject);
    }
}