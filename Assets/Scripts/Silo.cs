using UnityEngine;

public class Silo : MonoBehaviour, IInteractable
{
    public bool Interact(Player player)
    {
        if (player.heldItem != null && player.heldItem.Stack.item is FarmItem farmItem)
        {
            Farm.Instance.AddItem(farmItem.Kind, player.heldItem.Stack.count);
            StateManager.Instance.hasDeposited = true;
            Destroy(player.heldItem.gameObject);
            player.heldItem = null;
            return true;
        }
        return false;
    }
}
