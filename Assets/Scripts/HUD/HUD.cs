using UnityEngine;
using System.Collections.Generic;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject itemDisplayPrefab;
    [SerializeField] private Transform inventoryPanel;
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
    }

    void Update()
    {
        foreach (var e in Inventory)
        {
            if (!displays.ContainsKey(e.Key))
            {
                var display = Instantiate(itemDisplayPrefab, inventoryPanel).GetComponent<ItemDisplay>();
                display.Sprite = itemManager.Sprites[e.Key];
                displays.Add(e.Key, display);
            }

            displays[e.Key].Count = e.Value;
            displays[e.Key].Delta = Delta.ContainsKey(e.Key) ? Delta[e.Key] : 0;
            displays[e.Key].ShowDelta = ShowDelta;
        }
    }
}
