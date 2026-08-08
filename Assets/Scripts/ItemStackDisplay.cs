using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemStackDisplay : MonoBehaviour, IInteractable
{
    public ItemStack Stack;
    public bool ForceBackground = false;
    public bool ForceNoBackground = false;
    public bool ForceCount = false;
    public int SpriteLayer = 0;


    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private SpriteRenderer foregroundRenderer;
    [SerializeField] private SpriteRenderer itemRenderer;
    [SerializeField] private TMP_Text countText;


    void Update()
    {
        var enabled = Stack.item != null;
        backgroundRenderer.enabled = enabled;
        foregroundRenderer.enabled = enabled;
        itemRenderer.enabled = enabled;
        countText.enabled = enabled;
        if (!enabled) return;

        itemRenderer.sprite = Stack.item.Sprite;

        backgroundRenderer.sortingOrder = SpriteLayer;
        itemRenderer.sortingOrder = SpriteLayer + 1;
        foregroundRenderer.sortingOrder = SpriteLayer + 2;

        countText.enabled = Stack.count > 1 || ForceCount;
        countText.text = Stack.count.ToString();

        backgroundRenderer.enabled = (Stack.count > 1 || ForceBackground) && !ForceNoBackground;
        foregroundRenderer.enabled = backgroundRenderer.enabled;
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
