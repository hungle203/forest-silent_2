using UnityEngine;

public class BloodManager : MonoBehaviour
{
    public static BloodManager Instance;

    public GameObject bloodEffect;
    public GameObject bodyDecal;
    public GameObject groundDecal;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnBlood(RaycastHit hit)
    {
        // Máu phun
        Instantiate(
            bloodEffect,
            hit.point,
            Quaternion.LookRotation(hit.normal));

        // Máu bám lên người
        GameObject body = Instantiate(
            bodyDecal,
            hit.point + hit.normal * 0.01f,
            Quaternion.LookRotation(hit.normal));

        body.transform.SetParent(hit.collider.transform, true);

        Destroy(body, 60f);

        // Máu trên mặt đất
        if (Physics.Raycast(hit.point + Vector3.up,
            Vector3.down,
            out RaycastHit groundHit,
            5f))
        {
            GameObject ground = Instantiate(
                groundDecal,
                groundHit.point + groundHit.normal * 0.01f,
                Quaternion.LookRotation(groundHit.normal));

            Destroy(ground, 60f);
        }
    }
}