using System;
using UnityEngine;

public abstract class Item
{
    public abstract int MaxStackCount { get; }
    public abstract Sprite Sprite { get; }
}

[Serializable]
public class Axe : Item
{
    public override int MaxStackCount => 1;
    public override Sprite Sprite => Resources.Load<Sprite>("Items/Tools/axe");

    public override bool Equals(object obj) { return obj is Axe; }
    public override int GetHashCode() { return base.GetHashCode(); }
}
[Serializable]
public class Hoe : Item
{
    public override int MaxStackCount => 1;
    public override Sprite Sprite => Resources.Load<Sprite>("Items/Tools/hoe");

    public override bool Equals(object obj) { return obj is Hoe; }
    public override int GetHashCode() { return base.GetHashCode(); }
}
[Serializable]
public class Hammer : Item
{
    public override int MaxStackCount => 1;
    public override Sprite Sprite => Resources.Load<Sprite>("Items/Tools/hammer");

    public override bool Equals(object obj) { return obj is Hammer; }
    public override int GetHashCode() { return base.GetHashCode(); }
}

[Serializable]
public class Banner : Item
{
    public override int MaxStackCount => 1;
    public override Sprite Sprite => Resources.Load<Sprite>("Items/banner");

    public override bool Equals(object obj) { return obj is Banner; }
    public override int GetHashCode() { return base.GetHashCode(); }
}
