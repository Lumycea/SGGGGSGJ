using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    private static readonly int WalkHash = Animator.StringToHash("walk");
    private static readonly int XHash = Animator.StringToHash("x");
    private static readonly int YHash = Animator.StringToHash("y");

    private Animator animator;

    private Vector2 moveVector = Vector2.down;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        var moving = moveVector.magnitude > 0.1f;

        animator.SetBool(WalkHash, moving);

        if (moving)
        {
            animator.SetFloat(XHash, moveVector.x);
            animator.SetFloat(YHash, moveVector.y);
            GetComponent<SpriteRenderer>().flipX = moveVector.x > 0.1f;
        }

    }

    public void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>().normalized;
    }
}
