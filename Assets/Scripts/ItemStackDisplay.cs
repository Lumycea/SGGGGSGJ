using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemStackDisplay : MonoBehaviour, IInteractable
{
    public ItemStack Stack = new(FarmItem.Sugar, 1);
    public bool ForceBackground = false;

    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private SpriteRenderer itemRenderer;
    [SerializeField] private TMP_Text countText;

    private ItemManager itemManager;


    void Start()
    {
        itemManager = GameObject.FindWithTag("GameManager").GetComponent<ItemManager>();
    }

    void Update()
    {
        itemRenderer.sprite = itemManager.Sprites[Stack.item];

        countText.enabled = Stack.count > 1;
        countText.text = Stack.count.ToString();

        backgroundRenderer.enabled = Stack.count > 1 || ForceBackground;
    }

    public bool DecreaseCount(int amount)
    {
        if (Stack.count < amount) { return false; }


        Stack.count -= amount;

        if (Stack.count <= 0)
        {
            Destroy(gameObject);
            return true;
        }

        return false;
    }

    public void Interact(Player player)
    {
        print(player.heldItem);
        if (player.heldItem == null)
        {
            player.heldItem = this;
            transform.SetParent(player.playerObject.GetComponent<PlayerController>().dockingPoint.transform);
            transform.localPosition = Vector3.zero;
            return;
        }

        print(player.heldItem.Stack.item);
        if (player.heldItem.Stack.item == Stack.item)
        {
            player.heldItem.Stack.count += Stack.count;
            Destroy(gameObject);
            return;
        }
    }
}
