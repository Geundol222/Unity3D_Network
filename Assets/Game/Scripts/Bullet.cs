using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * moveSpeed;
        Destroy(gameObject, 3f);
    }

    public void ApplyLag(float lag)
    {
        transform.position += rb.velocity * lag;
    }
}
