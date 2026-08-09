using UnityEngine;

public class QuestPoint : MonoBehaviour, IInteractable
{
    [SerializeField] private int slotIndex;
    [SerializeField] GameObject itemStackPrefab;
    [SerializeField] ItemStackDisplay display;


    [SerializeField] private Sprite animalSprite, shopSprite;
    [SerializeField]
    private SpriteRenderer bubbleRenderer, animalRenderer, shopRenderer;

    void Start()
    {
        shopRenderer.sprite = shopSprite;
        animalRenderer.sprite = animalSprite;
        display.gameObject.SetActive(false);
    }

    void Update()
    {
        if (QuestManager.Instance.HasQuest(slotIndex))
        {
            var quest = QuestManager.Instance.Quest(slotIndex);
            display.Stack = quest.Stack;
            display.gameObject.SetActive(true);
            bubbleRenderer.enabled = true;
            animalRenderer.enabled = true;
        }
        else
        {
            display.gameObject.SetActive(false);
            bubbleRenderer.enabled = false;
            animalRenderer.enabled = false;
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
        if (playerState.heldItem == null && HasQuest())
        {
            GameObject ticket = Instantiate(itemStackPrefab, transform.position, Quaternion.identity);
            var stackDisplay = ticket.GetComponent<ItemStackDisplay>();
            stackDisplay.Stack = new ItemStack(GetTicket(), 1);
            playerState.SetItem(stackDisplay);
            StateManager.Instance.hasPickedTicket = true;
            return true;
        }
        else if (playerState.heldItem != null && playerState.heldItem.Stack.item is QuestPackage package)
        {
            if (DepositPackage(package))
            {
                Destroy(playerState.heldItem.gameObject);
                playerState.heldItem = null;
                StateManager.Instance.hasCompletedQuest = true;
                return true;
            }
        }
        return false;
    }
}
