using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    private Player playerState;

    void Start()
    {
        int playerId = GetComponent<PlayerInput>().playerIndex;
        PlayerManager playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();
        playerState = playerManager.players[playerId];
    }

    public void OnInteract()
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(interactionPoint.position);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                print($"Interacting with {interactable}");
                interactable.Interact(playerState);
                return;
            }
        }
    }
}
