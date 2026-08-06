using System;
using System.Collections.Generic;
using System.Linq;

public class Farm : SimulationEntity
{
    private int _wheat = 0;
    public int Wheat
    {
        get => _wheat; set
        {
            if (value < 0) { return; }
            _wheat = value;
        }
    }

    public void AddItem(FarmItemKind item, uint count)
    {
        if (!Inventory.ContainsKey(item)) { Inventory.Add(item, count); }
        else { Inventory[item] += count; }
    }

    public void RegisterAutomatedAction(Dictionary<FarmItemKind, int> delta)
    {
        foreach (var e in delta)
        {
            if (!AutomatedDelta.ContainsKey(e.Key)) { AutomatedDelta.Add(e.Key, e.Value); }
            else { AutomatedDelta[e.Key] += e.Value; }
        }
    }

    public override void PreTick()
    {
        base.PreTick();
        foreach (var i in Enum.GetValues(typeof(FarmItemKind)).Cast<FarmItemKind>())
        {
            AutomatedDelta.Clear();
        }
    }
    public override void Tick() { }
    public override void PostTick()
    {
        base.PostTick();
        foreach (var v in Inventory)
        {
            print("Inventory contains " + v.Value + " " + v.Key.ToString());
        }
    }

    public Dictionary<FarmItemKind, uint> Inventory { get; private set; } = new Dictionary<FarmItemKind, uint>();
    public Dictionary<FarmItemKind, int> AutomatedDelta { get; private set; } = new Dictionary<FarmItemKind, int>();

}
