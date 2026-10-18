using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float hitRadius = 0.15f;
    [SerializeField] private float maxDistance = 20f;

    private Vector2 direction;
    private Transform player;
    private float distanceTravelled;
    private bool isReturning;

    // PlayerShooter calls this when it creates the bullet.
    public void Launch(Vector2 shotDirection, Transform shooter)
    {
        direction = shotDirection.normalized;
        player = shooter;
    }

    private void Update()
    {
        if (player == null) return;

        float step = speed * Time.deltaTime;

        if (isReturning)
        {
            Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;

            if (toPlayer.magnitude < 0.3f)
            {
                Destroy(gameObject);
                return;
            }

            direction = toPlayer.normalized;
        }
        else
        {
            Vector2 nextPosition = (Vector2)transform.position + direction * step;
            Collider2D[] thingsInTheWay = Physics2D.OverlapCircleAll(nextPosition, hitRadius);

            foreach (Collider2D thing in thingsInTheWay)
            {
                if (thing.transform == player || thing.isTrigger) continue;

                isReturning = true;
                return;
            }

            transform.position = nextPosition;
            distanceTravelled += step;

            if (distanceTravelled >= maxDistance)
                isReturning = true;
        }

        transform.position += (Vector3)(direction * step);
    }
}
