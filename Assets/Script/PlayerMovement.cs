using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 9f;
    [SerializeField] private float groundCheckDistance = 0.08f;

    private Rigidbody2D body;
    private Collider2D playerCollider;
    private InputAction moveAction;
    private InputAction jumpAction;
    private bool isGrounded;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

        // Keep the character upright while the Rigidbody2D handles falling.
        body.freezeRotation = true;

        moveAction = InputSystem.actions?.FindAction("Player/Move");
        jumpAction = InputSystem.actions?.FindAction("Player/Jump");

        if (moveAction == null || jumpAction == null)
        {
            Debug.LogError("Could not find Player/Move or Player/Jump in the project Input Actions.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        moveAction?.Enable();
        jumpAction?.Enable();
    }

    private void Update()
    {
        isGrounded = CheckGrounded();

        if (isGrounded && jumpAction != null && jumpAction.WasPressedThisFrame())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        if (moveAction == null || body == null) return;

        float horizontal = moveAction.ReadValue<Vector2>().x;
        body.linearVelocity = new Vector2(horizontal * moveSpeed, body.linearVelocity.y);
    }

    private bool CheckGrounded()
    {
        if (body == null || playerCollider == null) return false;

        Vector2 feetPosition = new Vector2(
            playerCollider.bounds.center.x,
            playerCollider.bounds.min.y - 0.02f);

        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(feetPosition, groundCheckDistance);
        foreach (Collider2D nearbyCollider in nearbyColliders)
        {
            if (nearbyCollider != playerCollider && !nearbyCollider.isTrigger)
                return true;
        }

        return false;
    }

    private void OnDisable()
    {
        if (body != null)
            body.linearVelocity = new Vector2(0f, body.linearVelocity.y);
    }
}
