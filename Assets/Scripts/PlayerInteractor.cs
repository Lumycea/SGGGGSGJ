using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInteractor : MonoBehaviour
{
    public Transform interactionPoint;
    [SerializeField] private LayerMask interactorLayerMask;
    private Player playerState;
    public float interactionRadius = 0.5f;

    void Start()
    {
        int playerId = GetComponent<PlayerInput>().playerIndex;
        PlayerManager playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();
        playerState = playerManager.players[playerId];
    }

    public void OnInteract()
    {
        if (playerState.isInJail) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(interactionPoint.position, interactionRadius, interactorLayerMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                print(interactable);
                if (interactable.Interact(playerState)) return;
            }
        }

        if (playerState.heldItem != null)
        {
            if (playerState.heldItem.Stack.item is QuestTicket)
            {
                Destroy(playerState.heldItem.gameObject);
                playerState.heldItem = null;
            }
            else
            {
                playerState.DropItem();
            }
            return;
        }
    }

    public void OnSwipeLeft()
    {
        if (playerState.isInJail) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(interactionPoint.position, interactionRadius, interactorLayerMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                if (interactable.Swipe(playerState, IInteractable.Direction.Left)) return;
            }
        }
    }

    public void OnSwipeRight()
    {
        if (playerState.isInJail) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(interactionPoint.position, interactionRadius, interactorLayerMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                if (interactable.Swipe(playerState, IInteractable.Direction.Right)) return;
            }
        }
    }
}
