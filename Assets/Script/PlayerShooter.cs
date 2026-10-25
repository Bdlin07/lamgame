using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    private Bullet currentBullet;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Mouse.current == null || Keyboard.current == null) return;
        if (Mouse.current.leftButton.wasPressedThisFrame && currentBullet == null)
            Shoot();

        if (currentBullet != null && Keyboard.current.eKey.wasPressedThisFrame)
            currentBullet.ReturnToPlayer();
    }

    private void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Kéo prefab Bullet vào ô Bullet Prefab của Player Shooter.", this);
            return;
        }

        Camera camera = Camera.main;
        if (camera == null) return;
        Vector3 mousePosition = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0f;

        Vector2 direction = ((Vector2)mousePosition - (Vector2)transform.position).normalized;
        Vector3 startPosition = transform.position + (Vector3)(direction * 1.2f);

        GameObject bulletObject = Instantiate(bulletPrefab, startPosition, Quaternion.identity);
        currentBullet = bulletObject.GetComponent<Bullet>();

        if (currentBullet == null)
        {
            Debug.LogError("Prefab Bullet cần có component Bullet.", bulletObject);
            Destroy(bulletObject);
            return;
        }

        currentBullet.Launch(direction, transform);

        if (animator != null)
            animator.SetTrigger("Shoot");
    }
}
