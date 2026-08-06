using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Crop : SimulationEntity, IInteractable
{
    [SerializeField] private int GrowthProbability;
    [SerializeField] private Sprite[] Sprites;
    [SerializeField] private SpriteRenderer cropRenderer, groundRenderer;
    [SerializeField] private Field Field;
    [SerializeField] private GameObject itemStackPrefab;

    public FarmItem? Item { get; private set; }

    private bool isTiled = false;
    private uint growthStage = 0;

    void Update()
    {
        cropRenderer.enabled = Item != null;
        cropRenderer.sprite = Sprites[growthStage];

        groundRenderer.enabled = isTiled;
    }

    public override void Tick()
    {
        if (Item != null && growthStage < Sprites.Length - 1 && Random.Range(0, GrowthProbability) == 0)
        {
            growthStage += 1;
        }

    }

    public bool IsHarvestable() { return growthStage == Sprites.Length && Item != null; }
    public void Harvest() { growthStage = 0; }


    public bool IsTileable() { return !isTiled; }
    public void Tile() { isTiled = true; }

    public bool CanPlant(FarmItem item)
    {
        return isTiled &&
        Item == null &&
        Field.CanPlant(item) &&
        (
            item == FarmItem.MilkSeed ||
            item == FarmItem.CocoaSeed ||
            item == FarmItem.SugarSeed ||
            item == FarmItem.IceSeed
        );
    }
    public void Plant(FarmItem item)
    {
        switch (item)
        {
            case FarmItem.MilkSeed:
                Item = FarmItem.Milk;
                break;
            case FarmItem.CocoaSeed:
                Item = FarmItem.Cocoa;
                break;
            case FarmItem.SugarSeed:
                Item = FarmItem.Sugar;
                break;
            case FarmItem.IceSeed:
                Item = FarmItem.Ice;
                break;
        }
    }

    public void Interact(Player player)
    {
        if (IsHarvestable())
        {
            var prefab = Instantiate(itemStackPrefab, transform.position, Quaternion.identity);
            var stack = prefab.GetComponent<ItemStackDisplay>();
            stack.Stack = new ItemStack(Item ?? throw new System.Exception(), 1);
            Harvest();
            return;
        }

        if (player.heldItem != null && player.heldItem.Stack.item == FarmItem.Hoe && !isTiled)
        {
            Tile();
            return;
        }

        if (player.heldItem != null && CanPlant(player.heldItem.Stack.item))
        {
            Plant(player.heldItem.Stack.item);
            if (player.heldItem.DecreaseCount(1))
            {
                player.heldItem = null;
            }
            return;
        }
    }
}
