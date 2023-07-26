using Photon.Realtime;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private Player player;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * moveSpeed;
        Destroy(gameObject, 3f);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void ApplyLag(float lag)
    {
        transform.position += rb.velocity * lag;
    }

    public void GetScore()
    {
        // player 점수 추가
    }
}
