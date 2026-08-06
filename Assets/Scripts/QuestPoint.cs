using UnityEngine;

public class QuestPoint : MonoBehaviour, IInteractable
{
    [SerializeField] private int slotIndex;
    [SerializeField] GameObject itemStackPrefab;
    [SerializeField] ItemStackDisplay display;

    void Start()
    {
        display.gameObject.SetActive(false);
    }

    void Update()
    {
        if (QuestManager.Instance.PendingQuests[slotIndex] is ItemStack stack)
        {
            display.Stack = stack;
            display.gameObject.SetActive(true);
        }
        else
        {
            display.gameObject.SetActive(false);
        }
    }

    public bool HasQuest()
    {
        return QuestManager.Instance.PendingQuests[slotIndex] != null;
    }

    public Item GetTicket()
    {
        return new QuestTicket(QuestManager.Instance.PendingQuests[slotIndex] ?? throw new System.Exception());
    }

    public bool DepositPackage(QuestPackage package)
    {
        return QuestManager.Instance.DepositPackage(package, slotIndex);
    }

    public bool Interact(Player playerState)
    {
        if (playerState.heldItem == null && HasQuest())
        {
            GameObject ticket = Instantiate(itemStackPrefab, transform.position, Quaternion.identity);
            playerState.SetItem(ticket.GetComponent<ItemStackDisplay>());
            return true;
        }
        else if (playerState.heldItem != null && playerState.heldItem.Stack.item is QuestPackage package)
        {
            if (DepositPackage(package))
            {
                Destroy(playerState.heldItem.gameObject);
                playerState.heldItem = null;
                return true;
            }
        }
        return false;
    }
}
