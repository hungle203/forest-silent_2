using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 200f;
    public float damage = 25f;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

 private void OnTriggerEnter(Collider other)
{
    Debug.Log("Hit: " + other.name);

    ZombieAI zombie = other.GetComponentInParent<ZombieAI>();

    if (zombie != null)
    {
        zombie.TakeDamage(damage);

        Vector3 hitPoint = other.ClosestPoint(transform.position);
        Vector3 hitNormal = (hitPoint - zombie.transform.position).normalized;

        zombie.SpawnBlood(hitPoint, hitNormal);
    }

    Destroy(gameObject);
}


}