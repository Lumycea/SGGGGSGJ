using UnityEngine;

public class Silo : MonoBehaviour, IInteractable
{
    public bool Interact(Player player)
    {
        if (player.heldItem != null)
        {
            if (player.heldItem.Stack.item is FarmItem farmItem)
            {
                Farm.Instance.AddItem(farmItem.Kind, player.heldItem.Stack.count);
                StateManager.Instance.hasDeposited = true;
                Destroy(player.heldItem.gameObject);
                player.heldItem = null;
                return true;
            }
            else if (player.heldItem.Stack.item is QuestPackage package)
            {
                Farm.Instance.AddItem((package.Stack.item as FarmItem).Kind, package.Stack.count);
                Destroy(player.heldItem.gameObject);
                player.heldItem = null;
                return true;
            }
            else if (player.heldItem.Stack.item is QuestTicket ticket)
            {
                if (Farm.Instance.TryRemoveItem((ticket.Stack.item as FarmItem).Kind, ticket.Stack.count))
                {
                    var questPackage = new QuestPackage(ticket.Stack);
                    player.heldItem.Stack.item = questPackage;
                    StateManager.Instance.hasUsedTicket = true;
                }
                return true;
            }
        }
        return false;
    }
}
