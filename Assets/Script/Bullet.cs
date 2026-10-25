using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float hitRadius = 0.15f;
    [SerializeField] private float maxDistance = 20f;

    private Vector2 direction;
    private Transform player;
    private float distanceTravelled;

    public bool IsStopped { get; private set; }
    public void Launch(Vector2 shotDirection, Transform shooter)
    {
        direction = shotDirection.normalized;
        player = shooter;
    }

    private void Update()
    {
        if (player == null || IsStopped) return;

        float step = speed * Time.deltaTime;
        Vector2 nextPosition = (Vector2)transform.position + direction * step;
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(nextPosition, hitRadius);

        // Check each nearby collider until the bullet finds a real obstacle.
        foreach (Collider2D collider in nearbyColliders)
        {
            if (collider.transform == player || collider.isTrigger) continue;

            transform.position = nextPosition - direction * hitRadius;
            IsStopped = true;
            return;
        }

        transform.position = nextPosition;
        distanceTravelled += step;

        if (distanceTravelled >= maxDistance)
            IsStopped = true;
    }
}
