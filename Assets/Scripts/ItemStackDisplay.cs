using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemStackDisplay : MonoBehaviour, IInteractable
{
    public ItemStack Stack;
    public bool ForceBackground = false;
    public bool ForceNoBackground = false;
    public int SpriteLayer = 0;


    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private SpriteRenderer itemRenderer;
    [SerializeField] private TMP_Text countText;


    void Update()
    {
        itemRenderer.sprite = Stack.item.Sprite;

        backgroundRenderer.sortingOrder = SpriteLayer;
        itemRenderer.sortingOrder = SpriteLayer + 1;

        countText.enabled = Stack.count > 1;
        countText.text = Stack.count.ToString();

        backgroundRenderer.enabled = (Stack.count > 1 || ForceBackground) && !ForceNoBackground;
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

    public bool Interact(Player player)
    {
        if (player.heldItem == null)
        {
            player.SetItem(this);
            return true;
        }
        else if (player.heldItem.Stack.item.Equals(Stack.item))
        {
            player.heldItem.Stack.count += Stack.count;
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
