using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 9f;

    private Rigidbody2D body;
    private InputAction moveAction;
    private InputAction jumpAction;
    private readonly RaycastHit2D[] groundHits = new RaycastHit2D[1];

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;

        moveAction = InputSystem.actions?.FindAction("Player/Move");
        jumpAction = InputSystem.actions?.FindAction("Player/Jump");

        if (moveAction == null || jumpAction == null)
        {
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
        // Cast Player's collider a tiny bit downward to see if it touches the floor.
        bool isGrounded = body.Cast(Vector2.down, groundHits, 0.1f) > 0;

        if (isGrounded && jumpAction != null && jumpAction.WasPressedThisFrame())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        if (moveAction == null || body == null) return;

        float horizontal = moveAction.ReadValue<Vector2>().x;
        body.linearVelocity = new Vector2(horizontal * moveSpeed, body.linearVelocity.y);
    }

    private void OnDisable()
    {
        if (body != null)
            body.linearVelocity = new Vector2(0f, body.linearVelocity.y);
    }
}
