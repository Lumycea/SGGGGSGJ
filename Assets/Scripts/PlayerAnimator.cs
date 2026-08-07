using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerAnimator : MonoBehaviour
{
    private static readonly int HasBoxHash = Animator.StringToHash("has_box");
    private static readonly int HasAxeHash = Animator.StringToHash("has_axe");
    private static readonly int HasHoeHash = Animator.StringToHash("has_hoe");
    private static readonly int HasHammerHash = Animator.StringToHash("has_hammer");
    private static readonly int WalkHash = Animator.StringToHash("walk");
    private static readonly int XHash = Animator.StringToHash("x");
    private static readonly int YHash = Animator.StringToHash("y");

    private Animator animator;
    private Player player;

    private Vector2 moveVector = Vector2.down;

    private void Start()
    {
        animator = GetComponent<Animator>();

        int playerId = GetComponent<PlayerInput>().playerIndex;
        PlayerManager playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();
        player = playerManager.players[playerId];
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


        Item item = player.heldItem ? player.heldItem.Stack.item : null;
        bool hasAxe = item is Axe,
            hasHoe = item is Hoe,
            hasHammer = item is Hammer,
            hasBox = item != null && !(hasAxe || hasHoe || hasHammer);

        animator.SetBool(HasBoxHash, hasBox);
        animator.SetBool(HasAxeHash, hasAxe);
        animator.SetBool(HasHoeHash, hasHoe);
        animator.SetBool(HasHammerHash, hasHammer);
    }

    public void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>().normalized;
    }
}
