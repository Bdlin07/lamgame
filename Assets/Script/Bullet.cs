using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float returnSpeed = 10f;
    [SerializeField] private int damage = 1;

    private Rigidbody2D body;
    private Transform player;
    private bool isReturning;
    private Collider2D bulletCollider;
    public bool IsStopped { get; private set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<Collider2D>();

        if (body != null)
            body.freezeRotation = true;
    }

    public void Launch(Vector2 shotDirection, Transform shooter)
    {
        player = shooter;

        if (body != null)
        {
            body.gravityScale = 0f;
            body.bodyType = RigidbodyType2D.Dynamic;
            body.linearVelocity = shotDirection.normalized * speed;
        }
    }

    public void ReturnToPlayer()
    {
        if (player == null || body == null) return;

        isReturning = true;
        IsStopped = false;
        body.bodyType = RigidbodyType2D.Kinematic;
        body.gravityScale = 0f;
        body.linearVelocity = Vector2.zero;

        if (bulletCollider != null)
            bulletCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        if (!isReturning || player == null || body == null) return;

        Vector2 directionToPlayer = (Vector2)player.position - body.position;

        if (directionToPlayer.magnitude < 0.3f)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 nextPosition = body.position + directionToPlayer.normalized * returnSpeed * Time.fixedDeltaTime;
        body.MovePosition(nextPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isReturning || IsStopped || collision.transform == player) return;

        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
            health.TakeDamage(damage);

        IsStopped = true;
        body.linearVelocity = Vector2.zero;
        body.gravityScale = 0f;
        body.bodyType = RigidbodyType2D.Kinematic;

        if (bulletCollider != null)
            bulletCollider.enabled = false;
    }
}
