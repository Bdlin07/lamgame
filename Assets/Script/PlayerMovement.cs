using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D body;
    private InputAction moveAction;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions?.FindAction("Player/Move");

        if (moveAction == null)
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        moveAction?.Enable();
    }

    private void FixedUpdate()
    {
        float horizontal = moveAction.ReadValue<Vector2>().x;
        body.linearVelocity = new Vector2(horizontal * moveSpeed, body.linearVelocity.y);
    }

    private void OnDisable()
    {
        if (body != null)
            body.linearVelocity = new Vector2(0f, body.linearVelocity.y);
    }
}
