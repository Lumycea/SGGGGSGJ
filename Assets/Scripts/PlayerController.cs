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
    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerId = playerInput.playerIndex;
        playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();
        GetComponent<SpriteRenderer>().color = playerManager.players[playerId].playerColor;
        stateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<StateManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var device = playerInput.GetDevice<Gamepad>();
        if (device != null)
        {
            if (!playerManager.players[playerId].isInZone)
            {
                device.SetMotorSpeeds(0.6f, 0.2f);
            }
            else
            {
                device.SetMotorSpeeds(0f, 0f);
            }
        }

    }

    void FixedUpdate()
    {
        if (stateManager.isInGame && !playerManager.players[playerId].isInJail)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            playerManager.players[playerId].isInZone = true;
            print($"Player {playerId} entered zone");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            playerManager.players[playerId].isInZone = false;
            print($"Player {playerId} exited zone");
        }
    }
}
