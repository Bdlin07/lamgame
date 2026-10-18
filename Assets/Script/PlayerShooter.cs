using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    private InputAction attackAction;
    private InputAction aimAction;
    private Camera mainCamera;
    private Bullet currentBullet;

    private void Awake()
    {
        attackAction = InputSystem.actions?.FindAction("Player/Attack");
        aimAction = InputSystem.actions?.FindAction("UI/Point");
        mainCamera = Camera.main;

        if (attackAction == null || aimAction == null)
        {
            Debug.LogError("Could not find Player/Attack or UI/Point in the project Input Actions.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        attackAction?.Enable();
        aimAction?.Enable();
    }

    private void Update()
    {
        if (attackAction.WasPressedThisFrame() && currentBullet == null)
            Shoot();
    }

    private void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Drag your Bullet prefab into the Bullet Prefab field on PlayerShooter.", this);
            return;
        }

        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;

        Vector2 mouseScreenPosition = aimAction.ReadValue<Vector2>();
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        Vector2 direction = ((Vector2)mouseWorldPosition - (Vector2)transform.position).normalized;
        if (direction.sqrMagnitude < 0.01f) return;

        Vector3 spawnPosition = transform.position + (Vector3)(direction * 0.65f);
        GameObject bulletObject = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        currentBullet = bulletObject.GetComponent<Bullet>();

        if (currentBullet == null)
        {
            Debug.LogError("The Bullet prefab needs a Bullet component.", bulletObject);
            Destroy(bulletObject);
            return;
        }

        currentBullet.Launch(direction, transform);
    }
}
