using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Crop : SimulationEntity
{
    [SerializeField] private int GrowthProbability;
    [SerializeField] private Sprite[] Sprites;
    [SerializeField] private SpriteRenderer cropRenderer, groundRenderer;
    [SerializeField] private Field Field;

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

    public bool IsHarvestable() { return growthStage == Sprites.Length; }
    public void Harvest() { growthStage = 0; }


    public bool IsTileable() { return !isTiled; }
    public void Tile() { isTiled = true; }

    public bool CanPlant(FarmItem item) { return isTiled && Item == null && Field.CanPlant(item); }
    public void Plant(FarmItem item) { Item = item; }
}
