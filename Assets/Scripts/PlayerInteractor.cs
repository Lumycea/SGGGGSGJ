using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInteractor : MonoBehaviour
{
    public Transform interactionPoint;
    [SerializeField] private LayerMask interactorLayerMask;
    private Player playerState;
    public float interactionRadius = 0.5f;
    public float interactionFreezeTime = 0.5f;

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
                if (interactable.Interact(playerState))
                {
                    var playerControler = GetComponent<PlayerController>();
                    playerControler.StartCoroutine(playerControler.FreezePlayer(interactionFreezeTime));
                    StartCoroutine(QuickRumble());
                    return;
                }
            }
        }

        if (playerState.heldItem != null)
        {
            if (playerState.heldItem.Stack.item is QuestTicket)
            {
                playerState.DestroyHeldItem();
            }
            else
            {
                playerState.DropItem();
            }

            var playerControler = GetComponent<PlayerController>();
            playerControler.StartCoroutine(playerControler.FreezePlayer(interactionFreezeTime));
            StartCoroutine(QuickRumble());
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

    IEnumerator QuickRumble()
    {
        Gamepad gamepad = GetComponent<PlayerInput>().GetDevice<Gamepad>();
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0.25f, 0f);
            yield return new WaitForSeconds(0.1f);
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }
}
