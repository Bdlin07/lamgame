using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private int damage = 1;

    private Rigidbody2D body;
    private Transform player;
    private float nextAttackTime;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        GameObject playerObject = GameObject.Find("Player");

        if (playerObject != null)
            player = playerObject.transform;

        if (body != null)
            body.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (player == null || body == null) return;

        float distanceToPlayer = player.position.x - transform.position.x;
        float direction = 0f;

        if (Mathf.Abs(distanceToPlayer) > 0.5f)
            direction = Mathf.Sign(distanceToPlayer);

        body.linearVelocity = new Vector2(direction * speed, body.linearVelocity.y);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name != "Player" || Time.time < nextAttackTime) return;

        Health playerHealth = collision.gameObject.GetComponent<Health>();
        if (playerHealth == null) return;

        playerHealth.TakeDamage(damage);
        nextAttackTime = Time.time + 1.5f;
    }
}
