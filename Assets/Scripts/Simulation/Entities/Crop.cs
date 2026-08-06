using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Crop : SimulationEntity, IInteractable
{
    [SerializeField] private int GrowthProbability;
    [SerializeField] private Sprite[] Sprites;
    [SerializeField] private SpriteRenderer cropRenderer, groundRenderer;
    [SerializeField] private Field Field;
    [SerializeField] private GameObject itemStackPrefab;

    public FarmItemKind? Item { get; private set; }

    private bool isTilled = false;
    private uint growthStage = 0;

    void Update()
    {
        cropRenderer.enabled = Item != null;
        cropRenderer.sprite = Sprites[growthStage];

        groundRenderer.enabled = isTilled;
    }

    public override void Tick()
    {
        if (Item != null && growthStage < Sprites.Length - 1 && Random.Range(0, GrowthProbability) == 0)
        {
            growthStage += 1;
        }

    }

    public bool IsHarvestable() { return isTilled && growthStage == Sprites.Length - 1 && Item != null; }
    public void Harvest() { growthStage = 0; }


    public bool IsTillable() { return !isTilled; }
    public void Till()
    {
        isTilled = true;
        StateManager.Instance.hasTilled = true;
    }

    public bool CanPlant(FarmItemKind item) { return isTilled && Item == null && Field.CanPlant(item); }
    public void Plant(FarmItemKind item)
    {
        Item = item;
        StateManager.Instance.hasPlanted = true;
    }

    public bool Interact(Player player)
    {
        if (player.heldItem != null)
        {
            if (player.heldItem.Stack.item is Hoe)
            {
                if (IsTillable())
                {
                    Till();
                    return true;
                }
                else if (IsHarvestable() && Item is FarmItemKind kind)
                {
                    var prefab = Instantiate(itemStackPrefab, transform.position, Quaternion.identity);
                    var stack = prefab.GetComponent<ItemStackDisplay>();

                    stack.Stack = new ItemStack(new FarmItem(kind), 1);
                    player.SetItem(stack);

                    Harvest();
                    return true;
                }
            }
            else if (player.heldItem.Stack.item is FarmItem farmItem && IsHarvestable() && farmItem.Kind == Item)
            {
                player.heldItem.Stack.count += 1;
                Harvest();
                return true;
            }
            else if (player.heldItem.Stack.item is Seed seed && CanPlant(seed.Kind))
            {
                Plant(seed.Kind);
                if (player.heldItem.DecreaseCount(1))
                {
                    player.heldItem = null;
                }
                return true;
            }
        }
        return false;
    }
}
