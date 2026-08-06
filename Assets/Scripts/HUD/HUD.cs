using UnityEngine;
using System.Collections.Generic;

public class HUD : MonoBehaviour
{
    private Queue<ItemDisplay> AvailableDisplays;
    private readonly Dictionary<FarmItemKind, ItemDisplay> displays = new();
    private readonly uint[] usedSlots = { 0, 0, 0, 0 };
    private ItemManager itemManager;

    public Dictionary<FarmItemKind, int> Inventory = new();
    public Dictionary<FarmItemKind, int> Delta = new();
    public bool ShowDelta = false;

    public static HUD Instance { get; private set; }

    void Start()
    {
        Instance = this;

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
