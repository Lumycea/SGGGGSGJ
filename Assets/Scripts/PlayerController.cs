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
    public bool inPlayerSelect = false;
    public bool inGame = false;
    public float playerScrollCooldown = 0.5f;
    float lastScrollTime = 0f;

    void Start()
    {
        playerId = GetComponent<PlayerInput>().playerIndex;
        playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();
        GetComponent<SpriteRenderer>().color = playerManager.players[playerId].playerColor;
    }

    void FixedUpdate()
    {
        if (inGame)
        {
            transform.Translate(playerSpeed * Time.fixedDeltaTime * moveVector);
        }
    }

    public void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>().normalized;
    }

    public void OnNavigate(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>().normalized;

        if (inPlayerSelect)
        {
            if (Time.time - lastScrollTime >= playerScrollCooldown)
            {
                Player player = playerManager.players[playerId];
                if (inputVector.x > 0.5f)
                {
                    player.playerColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
                    Debug.Log("Navigate Right");
                    lastScrollTime = Time.time;
                }
                else if (inputVector.x < -0.5f)
                {
                    player.playerColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
                    Debug.Log("Navigate Left");
                    lastScrollTime = Time.time;
                }
            }
        }
    }

    public void OnDisconnect(InputValue value)
    {
        if (inPlayerSelect && value.isPressed)
        {
            DisconnectPlayer();
        }
    }

    public void DisconnectPlayer()
    {
        Destroy(gameObject);
    }
}
