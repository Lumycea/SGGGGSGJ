using System;
using System.Collections.Generic;
using System.Linq;

public class Farm : SimulationEntity
{
    public void AddItem(FarmItem item, uint count)
    {
        if (!Inventory.ContainsKey(item)) { Inventory.Add(item, count); }
        else { Inventory[item] += count; }
    }

    public void RegisterAutomatedAction(Dictionary<FarmItem, int> delta)
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
        foreach (var i in Enum.GetValues(typeof(FarmItem)).Cast<FarmItem>())
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

    public Dictionary<FarmItem, uint> Inventory { get; private set; } = new Dictionary<FarmItem, uint>();
    public Dictionary<FarmItem, int> AutomatedDelta { get; private set; } = new Dictionary<FarmItem, int>();

}
