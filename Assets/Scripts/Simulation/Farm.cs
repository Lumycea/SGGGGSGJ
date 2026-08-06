using System;
using System.Collections.Generic;
using System.Linq;

public class Farm : SimulationEntity
{
    private readonly Dictionary<FarmItemKind, int> inventory = new();
    private readonly Dictionary<FarmItemKind, int> delta = new();

    private int _wheat = 0;
    public int Wheat
    {
        get => _wheat; set
        {
            if (value < 0) { return; }
            _wheat = value;
        }
    }

    public static Farm Instance { get; private set; }

    void Start()
    {
        Instance = this;
    }

    public void AddItem(FarmItemKind item, int count)
    {
        if (!inventory.ContainsKey(item)) { inventory.Add(item, count); }
        else { inventory[item] += count; }
    }

    public void RegisterAutomatedAction(Dictionary<FarmItemKind, int> delta)
    {
        foreach (var e in delta)
        {
            if (!this.delta.ContainsKey(e.Key)) { this.delta.Add(e.Key, e.Value); }
            else { this.delta[e.Key] += e.Value; }
        }
    }

    public override void PreTick()
    {
        base.PreTick();
        foreach (var i in Enum.GetValues(typeof(FarmItemKind)).Cast<FarmItemKind>())
        {
            delta.Clear();
        }
    }
    public override void Tick() { }
    public override void PostTick()
    {
        base.PostTick();

        HUD.Instance.Inventory.Clear();
        HUD.Instance.Delta.Clear();
        HUD.Instance.ShowDelta = StateManager.Instance.showDelta;

        foreach (var e in inventory) { HUD.Instance.Inventory.Add(e.Key, e.Value); }
        foreach (var e in delta) { HUD.Instance.Delta.Add(e.Key, e.Value); }
    }
}
