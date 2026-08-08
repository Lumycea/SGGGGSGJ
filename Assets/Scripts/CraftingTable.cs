using UnityEngine;

public class CraftingTable : MonoBehaviour, IInteractable
{
    public static CraftingTable Instance;

    [SerializeField] private ItemStackDisplay input0, input1, output;
    [SerializeField] GameObject itemStackPrefab;
    private int selectedIndex = 0;

    void Start()
    {
        Instance = this;
        RefreshView();
    }

    private ItemStack? Craft()
    {
        var recipe = ItemManager.Instance.AvailableRecipes[selectedIndex];

        if (Farm.Instance.TryRemovePair(recipe.Items[0], recipe.Items[1]))
        {
            StateManager.Instance.hasCrafted = true;
            return new ItemStack(new FarmItem(recipe.Output), 1);
        }

        return null;
    }

    public bool Interact(Player playerState)
    {
        if (ItemManager.Instance.AvailableRecipes.Count == 0) { return false; }

        if (playerState.heldItem == null)
        {
            var item = Craft();
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
            if (playerState.heldItem.Stack.item is FarmItem f && f.Kind == ItemManager.Instance.AvailableRecipes[selectedIndex].Output)
            {
                if (Craft() != null)
                {
                    playerState.heldItem.Stack.count += 1;
                    return true;
                }
            }
        }
        return false;
    }

    public bool Swipe(Player playerState, IInteractable.Direction direction)
    {
        if (ItemManager.Instance.AvailableRecipes.Count == 0) { return false; }

        if (direction == IInteractable.Direction.Left)
        {
            selectedIndex = (selectedIndex + ItemManager.Instance.AvailableRecipes.Count - 1) % ItemManager.Instance.AvailableRecipes.Count;
            RefreshView();
            return true;
        }
        else if (direction == IInteractable.Direction.Right)
        {
            selectedIndex = (selectedIndex + 1) % ItemManager.Instance.AvailableRecipes.Count;
            RefreshView();
            return true;
        }

        return false;
    }

    void RefreshView()
    {
        if (ItemManager.Instance.AvailableRecipes.Count == 0)
        {
            input0.gameObject.SetActive(false);
            input1.gameObject.SetActive(false);
            output.gameObject.SetActive(false);
            return;
        }

        input0.gameObject.SetActive(true);
        input1.gameObject.SetActive(true);
        output.gameObject.SetActive(true);

        if (selectedIndex >= ItemManager.Instance.AvailableRecipes.Count) { selectedIndex = 0; }

        var recipe = ItemManager.Instance.AvailableRecipes[selectedIndex];

        input0.Stack = new ItemStack(new FarmItem(recipe.Items[0]), 1);
        input1.Stack = new ItemStack(new FarmItem(recipe.Items[1]), 1);
        output.Stack = new ItemStack(new FarmItem(recipe.Output), 1);
    }

    public void OnUpgrade()
    {
        RefreshView();
    }
}
