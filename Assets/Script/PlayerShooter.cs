using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float pickupDistance = 1.2f;

    private Bullet currentBullet;

    private void Update()
    {
        if (Mouse.current == null || Keyboard.current == null) return;

        // Chuột trái bắn, nhưng chỉ khi chưa có viên đạn ngoài màn hình.
        if (Mouse.current.leftButton.wasPressedThisFrame && currentBullet == null)
            Shoot();

        // Đứng gần viên đạn đã dừng rồi nhấn E để nhặt.
        if (currentBullet != null && currentBullet.IsStopped && Keyboard.current.eKey.wasPressedThisFrame)
        {
            float distance = Vector2.Distance(transform.position, currentBullet.transform.position);
            if (distance <= pickupDistance)
            {
                Destroy(currentBullet.gameObject);
                currentBullet = null;
            }
        }
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

        // Đổi vị trí chuột trên màn hình thành vị trí trong game.
        Vector3 mousePosition = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0f;

        Vector2 direction = ((Vector2)mousePosition - (Vector2)transform.position).normalized;
        Vector3 startPosition = transform.position + (Vector3)(direction * 0.65f);

        GameObject bulletObject = Instantiate(bulletPrefab, startPosition, Quaternion.identity);
        currentBullet = bulletObject.GetComponent<Bullet>();

        if (currentBullet == null)
        {
            Debug.LogError("Prefab Bullet cần có component Bullet.", bulletObject);
            Destroy(bulletObject);
            return;
        }

        currentBullet.Launch(direction, transform);
    }
}
