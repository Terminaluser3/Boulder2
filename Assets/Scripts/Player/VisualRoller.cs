using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// The player GO has it's rotation disabled to allow movement interpolation.
/// To allow rolling animation, the main sphere does not render and is only used for hitbox and movement.
/// This GO is used for visual. iT always maps position to the main GO.
/// </summary>
public class VisualRoller : MonoBehaviour
{
    private Rigidbody parentRigidBody;
    private float sphereRadius;

    private void Start()
    {
        parentRigidBody = GetComponentInParent<Rigidbody>();

        // Calc sphere's radius from it's collider to ensure accurate rolling speed.
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            // Radius is hald the diameter, scale affects collider size.
            sphereRadius = sphereCollider.radius * transform.lossyScale.x;
        }
        else
        {
            // If no collider.
            Debug.LogWarning("No collider");
            sphereRadius = 0.5f;
        }
    }

    private void Update()
    {
        if (parentRigidBody == null) return;

        // Calc distance sphere has travelled this frame.
        float distance = parentRigidBody.linearVelocity.magnitude * Time.deltaTime;

        // Calc rotation angle based on distance and radius.
        // Circumference = 2* pi * r . Rotation in r = d / r .
        float rotationInRadians = distance / sphereRadius;
        float rotationInDegrees = rotationInRadians * Mathf.Rad2Deg;

        // Determine axis of rotation. Should be perpendicular to direction of movement.
        // Vector3.Cross gives vector perpendicular to two input vectors.
        Vector3 rotationAxis = Vector3.Cross(parentRigidBody.linearVelocity.normalized, Vector3.up).normalized;

        // Apply rotation to visual sphere, regardless of parent rotation.
        transform.Rotate(rotationAxis, rotationInDegrees, Space.World);
    }
}

// @Author F.B.