using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUD : MonoBehaviour
{
    private Queue<ItemDisplay> AvailableDisplays;
    private Dictionary<FarmItem, ItemDisplay> displays = new Dictionary<FarmItem, ItemDisplay>();
    private uint[] usedSlots = { 0, 0, 0, 0 };
    private ItemManager itemManager;

    public Dictionary<FarmItem, uint> Inventory = new Dictionary<FarmItem, uint>();
    public Dictionary<FarmItem, int> Delta = new Dictionary<FarmItem, int>();
    public bool ShowDelta = false;

    void Start()
    {
        Inventory.Add(FarmItem.Milk, 2);
        Inventory.Add(FarmItem.Sugar, 4);
        Inventory.Add(FarmItem.Popsicle, 10);        

        Delta.Add(FarmItem.Milk, -1);
        Delta.Add(FarmItem.Sugar, +3);
        Delta.Add(FarmItem.Popsicle, +1);

        itemManager = GameObject.FindWithTag("GameManager").GetComponent<ItemManager>();
        AvailableDisplays = new Queue<ItemDisplay>(GetComponentsInChildren<ItemDisplay>());

        foreach(var d in AvailableDisplays)
        {
            d.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        foreach(var e in Inventory)
        {
            var tier = e.Key.GetTier();
            if(!displays.ContainsKey(e.Key))
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
            if(ShowDelta)
            {
                display.Delta = Delta[e.Key];
            }
        }
    }
}
