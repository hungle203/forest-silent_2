using UnityEngine;

public class GunFollow : MonoBehaviour
{
    public Transform cameraTransform;

    [Header("Offset")]
    public Vector3 positionOffset = new Vector3(0.25f, -0.2f, 0.5f);

    [Header("Rotation Offset")]
    public Vector3 rotationOffset = new Vector3(0f, -90f, 0f);

    [Header("Smooth")]
    public float positionSpeed = 20f;
    public float rotationSpeed = 20f;

    void LateUpdate()
    {
        // Vị trí trước camera
        Vector3 targetPos =
            cameraTransform.position
            + cameraTransform.right * positionOffset.x
            + cameraTransform.up * positionOffset.y
            + cameraTransform.forward * positionOffset.z;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            positionSpeed * Time.deltaTime);

        // Xoay theo camera + xoay bù cho model súng
        Quaternion targetRot =
            cameraTransform.rotation *
            Quaternion.Euler(rotationOffset);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime);
    }
}