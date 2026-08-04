using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Crop : SimulationEntity
{
    [SerializeField] private int GrowthProbability;
    [SerializeField] private Sprite[] Sprites;

    public FarmItem? Item { get; private set; } = null;
    public bool IsTiled { get; private set; } = false;
    private uint growthStage = 0;

    void Start() { }

    void Update()
    {
        GetComponent<SpriteRenderer>().enabled = Item != null;
        GetComponent<SpriteRenderer>().sprite = Sprites[growthStage];

        GetComponentInChildren<SpriteRenderer>().enabled = IsTiled;
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


    public bool IsTileable() { return !IsTiled; }
    public void Tile() { IsTiled = true; }

    public bool CanPlant(FarmItem item) { return IsTiled && Item == null; }
    public void Plant(FarmItem item) { Item = item; }
}
