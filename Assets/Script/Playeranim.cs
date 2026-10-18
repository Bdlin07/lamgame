using UnityEngine;
using UnityEngine.InputSystem;

public class Playeranim : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;

    InputAction moveAction;
    InputAction jumpAction;

    void Awake()
    {
        if(anim == null)
        {
            anim = GetComponent<Animator>();
        }

        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();

        if(spriteRenderer != null && move.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        if(spriteRenderer != null && move.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        if(anim == null) return;

        if(Mathf.Abs(move.x) > 0.01f)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if(jumpAction.WasPressedThisFrame())
        {
            anim.SetTrigger("Jump");
        }
    }
}
