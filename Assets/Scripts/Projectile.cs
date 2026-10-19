using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("References")] [Tooltip("The Rigidbody use to move the projectile through the scene.")] [SerializeField]
    private Rigidbody projectileRigidBody;

    [Header("Movement")] [SerializeField] private float movementSpeed = 40f;
    [SerializeField] private float lifetime = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        Destroy(gameObject, lifetime);
    }

    public void Initialize(Vector3 direction)
    {
        Vector3 normalizedDirection = direction.normalized;
        transform.forward = normalizedDirection;

        projectileRigidBody.linearVelocity = normalizedDirection * movementSpeed;
    }

}
