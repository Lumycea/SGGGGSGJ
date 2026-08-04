using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 5f;
    Vector2 inputVector;
    PlayerManager playerManager;

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();
        GetComponent<SpriteRenderer>().color = playerManager.players.Find(p => p.playerId == GetComponent<PlayerInput>().playerIndex).playerColor;
    }

    void FixedUpdate()
    {
        transform.Translate(playerSpeed * Time.fixedDeltaTime * inputVector);
    }

    public void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>().normalized;
    }

    public void DisconnectPlayer()
    {
        Destroy(gameObject);
    }
}
