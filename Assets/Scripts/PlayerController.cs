using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    int playerId;
    public float playerSpeed = 5f;
    private Vector2 inputVector;
    PlayerManager playerManager;
    private StateManager stateManager;
    private Rigidbody2D rb;
    public Transform nose;
    public float noseDistance = 0.5f;
    public GameObject dockingPoint;

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
        inputVector = value.Get<Vector2>();
    }

    private void MovePlayer()
    {
        rb.MovePosition(rb.position + inputVector * playerSpeed * Time.fixedDeltaTime);
        if (inputVector.magnitude > 0.1f)
        {
            nose.position = rb.position + inputVector.normalized * noseDistance;
        }
    }
}
