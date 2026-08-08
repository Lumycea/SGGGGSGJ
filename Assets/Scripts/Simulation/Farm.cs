using System;
using System.Collections.Generic;
using System.Linq;

public class Farm : SimulationEntity
{
    public Dictionary<FarmItemKind, int> Inventory { get; private set; } = new();
    public Dictionary<FarmItemKind, int> Delta { get; private set; } = new();

    public static Farm Instance { get; private set; }

    void Start()
    {
        Instance = this;
    }

    public void AddItem(FarmItemKind item, int count)
    {
        if (!Inventory.ContainsKey(item)) { Inventory.Add(item, count); }
        else { Inventory[item] += count; }

        UpdateDelta(item, count);
    }

    public bool TryRemoveItem(FarmItemKind item, int count)
    {
        if (!Inventory.ContainsKey(item) || Inventory[item] < count) return false;
        Inventory[item] -= count;
        UpdateDelta(item, -count);
        return true;
    }

    public bool TryRemovePair(FarmItemKind a, FarmItemKind b)
    {
        if (a == b) { return TryRemoveItem(a, 2); }

        if (!Inventory.ContainsKey(a) || Inventory[a] == 0) { return false; }
        if (!Inventory.ContainsKey(b) || Inventory[b] == 0) { return false; }

        Inventory[a] -= 1;
        Inventory[b] -= 1;

        return true;
    }

    private void UpdateDelta(FarmItemKind item, int count)
    {
        if (!Delta.ContainsKey(item)) { Delta.Add(item, count); }
        else { Delta[item] += count; }
    }

    public override void PreTick()
    {
        base.PreTick();
        foreach (var e in Delta.Keys.ToList())
        {
            Delta[e] = 0;
        }
    }
    public override void Tick() { }
    public override void PostTick()
    {
        base.PostTick();

        HUD.Instance.Inventory.Clear();
        HUD.Instance.Delta.Clear();
        HUD.Instance.ShowDelta = StateManager.Instance.showDelta;

        foreach (var e in Inventory) { HUD.Instance.Inventory.Add(e.Key, e.Value); }
        foreach (var e in Delta) { HUD.Instance.Delta.Add(e.Key, e.Value); }
    }
}
