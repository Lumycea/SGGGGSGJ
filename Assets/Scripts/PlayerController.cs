using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    int playerId;
    public float playerSpeed = 5f;
    Vector2 moveVector;
    PlayerManager playerManager;
    private StateManager stateManager;

    void Start()
    {
        playerId = GetComponent<PlayerInput>().playerIndex;
        playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();
        GetComponent<SpriteRenderer>().color = playerManager.players[playerId].playerColor;
        stateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<StateManager>();
    }

    void FixedUpdate()
    {
        if (stateManager.isInGame)
        {
            transform.Translate(playerSpeed * Time.fixedDeltaTime * moveVector);
        }
    }

    public void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>().normalized;
        Debug.Log("Player " + (playerId + 1) + " Move: " + moveVector);
    }
}
