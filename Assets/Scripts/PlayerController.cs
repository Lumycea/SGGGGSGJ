using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    int playerId;
    public float playerSpeed = 5f;
    Vector2 moveVector;
    PlayerManager playerManager;
    private StateManager stateManager;
    private Rigidbody2D rb;
    public Transform nose;

    void Start()
    {
        playerId = GetComponent<PlayerInput>().playerIndex;
        playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();
        GetComponent<SpriteRenderer>().color = playerManager.players[playerId].playerColor;
        stateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<StateManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (stateManager.isInGame)
        {
            MovePlayer();
        }
    }

    public void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>().normalized;
    }

    private void MovePlayer()
    {
        rb.MovePosition(rb.position + moveVector * playerSpeed * Time.fixedDeltaTime);
        nose.position = rb.position + moveVector * 0.5f;
    }
}
