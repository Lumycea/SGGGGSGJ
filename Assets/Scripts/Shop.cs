using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemStackDisplay Input;
    [SerializeField] private TMP_Text WheatInput;
    [SerializeField] private SpriteRenderer Output;
    [SerializeField] private GameObject itemStackPrefab;

    private readonly List<ShopEntry> entries = new();
    private int selectedIndex = 0;

    public static Shop Instance;

    void Start()
    {
        Instance = this;

        entries.Add(new ShopEntry(3, null, new ShopEntryItem(new ItemStack(new Seed(FarmItemKind.Sugar), 1)), true));
        entries.Add(new ShopEntry(10, new ItemStack(new FarmItem(FarmItemKind.Sugar), 5), new ShopEntryUpgrade(), true));

        RefreshView();
    }

    void RefreshView()
    {
        var entry = entries[selectedIndex];
        WheatInput.text = entry.wheatInput.ToString();

        if (entry.itemInput is ItemStack stack)
        {
            Input.Stack = stack;
            Input.gameObject.SetActive(true);
        }
        else
        {
            Input.gameObject.SetActive(false);
        }

        Output.sprite = entry.output.Sprite;
    }

    public void SwipeLeft()
    {
        selectedIndex = (selectedIndex + entries.Count - 1) % entries.Count;
        RefreshView();
    }
    public void SwipeRight()
    {
        selectedIndex = (selectedIndex + 1) % entries.Count;
        RefreshView();
    }

    public void AddEntry(ShopEntry entry) { entries.Add(entry); }

    public ItemStack? CurrentTarget()
    {
        var entry = entries[selectedIndex];
        return entry.output is ShopEntryItem e ? e.Stack : null;
    }

    public ItemStack? Buy()
    {
        var entry = entries[selectedIndex];

        if (StateManager.Instance.Wheat < entry.wheatInput) return null;
        if (entry.itemInput is ItemStack stack && !Farm.Instance.TryRemoveItem((stack.item as FarmItem).Kind, stack.count)) return null;

        StateManager.Instance.Wheat -= entry.wheatInput;

        ItemStack? ret = CurrentTarget();
        if (entry.output is ShopEntryUpgrade)
        {
            StateManager.Instance.UpgradeTier();
        }
        if (entry.output is ShopEntryFree)
        {
            PlayerManager.Instance.ReleasePlayer();
        }

        if (!entry.repeatable)
        {
            entries.RemoveAt(selectedIndex);
            selectedIndex = 0;
        }

        return ret;
    }

    public bool Swipe(Player playerState, IInteractable.Direction direction)
    {
        if (direction == IInteractable.Direction.Left)
        {
            SwipeLeft();
            return true;
        }
        else if (direction == IInteractable.Direction.Right)
        {
            SwipeRight();
            return true;
        }

        return false;
    }

    public bool Interact(Player playerState)
    {
        if (playerState.heldItem == null)
        {
            var item = Buy();
            if (item != null)
            {
                var itemObject = Instantiate(itemStackPrefab, transform.position, Quaternion.identity);
                var stack = itemObject.GetComponent<ItemStackDisplay>();
                stack.Stack = item.Value;
                playerState.SetItem(stack);
                return true;
            }
        }
        else
        {
            if (playerState.heldItem.Stack.item == CurrentTarget()?.item)
            {
                if (Buy() != null)
                {
                    playerState.heldItem.Stack.count += 1;
                    return true;
                }
            }
        }
        return false;
    }
}

public readonly struct ShopEntry
{
    public readonly int wheatInput;
    public readonly ItemStack? itemInput;
    public readonly ShopOutput output;
    public readonly bool repeatable;

    public ShopEntry(int _wheatInput, ItemStack? _itemInput, ShopOutput _output, bool _repeatable)
    {
        output = _output;
        repeatable = _repeatable;
        wheatInput = _wheatInput;
        itemInput = _itemInput;
    }
}

public abstract class ShopOutput
{
    abstract public Sprite Sprite { get; }
}

public class ShopEntryItem : ShopOutput
{
    public ShopEntryItem(ItemStack stack) { Stack = stack; }

    public ItemStack Stack;
    public override Sprite Sprite => Stack.item.Sprite;
}
public class ShopEntryUpgrade : ShopOutput
{
    public override Sprite Sprite => Resources.Load<Sprite>("Items/upgrade");
}
public class ShopEntryFree : ShopOutput
{
    public override Sprite Sprite => Resources.Load<Sprite>("Items/Prize_Ticket");
}
