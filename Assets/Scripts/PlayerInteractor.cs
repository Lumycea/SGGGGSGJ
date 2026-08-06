using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private LayerMask interactorLayerMask;
    private Player playerState;

    void Start()
    {
        int playerId = GetComponent<PlayerInput>().playerIndex;
        PlayerManager playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();
        playerState = playerManager.players[playerId];
    }

    public void OnInteract()
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(interactionPoint.position, interactorLayerMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                if (interactable.Interact(playerState)) return;
            }
        }

        if (playerState.heldItem != null)
        {
            if (playerState.heldItem.Stack.item is QuestTicket)
            {
                Destroy(playerState.heldItem.gameObject);
            }
            else
            {
                playerState.heldItem.transform.SetParent(null);
                playerState.heldItem.transform.position = interactionPoint.position;
                playerState.heldItem.gameObject.layer = LayerMask.NameToLayer("Interactable");
            }
            playerState.heldItem = null;
            return;
        }
    }

    public void OnSwipeLeft()
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(interactionPoint.position, interactorLayerMask);
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
        Collider2D[] colliders = Physics2D.OverlapPointAll(interactionPoint.position, interactorLayerMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                if (interactable.Swipe(playerState, IInteractable.Direction.Right)) return;
            }
        }
    }
}
