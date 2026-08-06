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

    public bool CanPlant(FarmItemKind item) { return isTiled && Item == null && Field.CanPlant(item); }
    public void Plant(FarmItemKind item) { Item = item; }

    public void Interact(Player player)
    {
        if (IsHarvestable() && Item is FarmItemKind kind)
        {
            var prefab = Instantiate(itemStackPrefab, transform.position, Quaternion.identity);
            var stack = prefab.GetComponent<ItemStackDisplay>();

            stack.Stack = new ItemStack(new FarmItem(kind), 1);
            Harvest();
            return;
        }

        if (player.heldItem != null && player.heldItem.Stack.item is Hoe && !isTiled)
        {
            Tile();
            return;
        }

        if (player.heldItem != null && player.heldItem.Stack.item is Seed seed && CanPlant(seed.Kind))
        {
            Plant(seed.Kind);
            if (player.heldItem.DecreaseCount(1))
            {
                player.heldItem = null;
            }
            return;
        }
    }
}
