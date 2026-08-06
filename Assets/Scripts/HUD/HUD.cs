using UnityEngine;
using System.Collections.Generic;

public class HUD : MonoBehaviour
{
    private Queue<ItemDisplay> AvailableDisplays;
    private Dictionary<FarmItemKind, ItemDisplay> displays = new Dictionary<FarmItemKind, ItemDisplay>();
    private uint[] usedSlots = { 0, 0, 0, 0 };
    private ItemManager itemManager;

    public Dictionary<FarmItemKind, int> Inventory = new Dictionary<FarmItemKind, int>();
    public Dictionary<FarmItemKind, int> Delta = new Dictionary<FarmItemKind, int>();
    public bool ShowDelta = false;

    public static HUD Instance { get; private set; }

    void Start()
    {
        Instance = this;

        Inventory.Add(FarmItemKind.Milk, 2);
        Inventory.Add(FarmItemKind.Sugar, 4);
        Inventory.Add(FarmItemKind.Popsicle, 10);

        Delta.Add(FarmItemKind.Milk, -1);
        Delta.Add(FarmItemKind.Sugar, +3);
        Delta.Add(FarmItemKind.Popsicle, +1);

        itemManager = GameObject.FindWithTag("GameManager").GetComponent<ItemManager>();
        AvailableDisplays = new Queue<ItemDisplay>(GetComponentsInChildren<ItemDisplay>());

        foreach (var d in AvailableDisplays)
        {
            d.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        foreach (var e in Inventory)
        {
            var tier = e.Key.GetTier();
            if (!displays.ContainsKey(e.Key))
            {
                var d = AvailableDisplays.Dequeue();
                d.gameObject.SetActive(true);
                d.gameObject.transform.Translate(usedSlots[tier] * 200, tier * -50, 0);
                usedSlots[tier] += 1;
                displays.Add(e.Key, d);
            }

            var display = displays[e.Key];
            display.Sprite = itemManager.Sprites[e.Key];
            display.Count = e.Value;
            display.ShowDelta = ShowDelta;
            if (ShowDelta)
            {
                display.Delta = Delta[e.Key];
            }
        }
    }
}
