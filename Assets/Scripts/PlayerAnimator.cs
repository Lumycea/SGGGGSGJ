using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    private static readonly int DirectionHash = Animator.StringToHash("direction");
    private static readonly int WalkHash = Animator.StringToHash("walk");

    private Animator animator;

    Vector2 moveVector;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        print(moveVector);
        animator.SetBool(WalkHash, moveVector.magnitude > 0.1f);

        var angle = Vector2.Angle(moveVector, Vector2.right);

        print(angle);

        int direction = 0;
        bool flipped = false;
        if (45 <= angle && angle <= 135) { direction = 1; }
        if (135 < angle && angle < 225) { direction = 2; }
        if (angle < 45 || angle > 315) { direction = 2; flipped = true; }

        print(direction);
        print(flipped);

        animator.SetInteger(DirectionHash, direction);
        GetComponent<SpriteRenderer>().flipX = flipped;
    }

    public void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>().normalized;
    }
}
