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
        if (QuestManager.Instance.HasQuest(slotIndex))
        {
            var quest = QuestManager.Instance.Quest(slotIndex);
            display.Stack = quest.Stack;
            display.gameObject.SetActive(true);
        }
        else
        {
            display.gameObject.SetActive(false);
        }
    }

    public bool HasQuest()
    {
        return QuestManager.Instance.HasQuest(slotIndex);
    }

    public Item GetTicket()
    {
        return new QuestTicket((QuestManager.Instance.Quest(slotIndex) ?? throw new System.Exception()).Stack);
    }

    public bool DepositPackage(QuestPackage package)
    {
        return QuestManager.Instance.DepositPackage(package, slotIndex);
    }

    public bool Interact(Player playerState)
    {
        print("interacting");
        if (playerState.heldItem == null && HasQuest())
        {
            GameObject ticket = Instantiate(itemStackPrefab, transform.position, Quaternion.identity);
            var stackDisplay = ticket.GetComponent<ItemStackDisplay>();
            stackDisplay.Stack = new ItemStack(GetTicket(), 1);
            playerState.SetItem(stackDisplay);
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
