using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public enum FarmItemKind
{
    Sugar,
    Ice,
    Cocoa,
    Milk,
    Candy,
    Chantilly,
    DarkChocolate,
    Popsicle,
    Milkshake,
    Sorbet,
    Chocolate,
    SweetenedCondensedMilk,
    Magnum,
    IceCream,
    Smarties,
}

[Serializable]
public class FarmItem : Item
{
    public FarmItem() { }
    public FarmItem(FarmItemKind kind) { Kind = kind; }

    public FarmItemKind Kind;

    public override int MaxStackCount => 4;
    public override Sprite Sprite => ItemManager.Instance.Sprites[Kind];
    public override bool Equals(object obj) { return obj is FarmItem s && s.Kind == Kind; }
    public override int GetHashCode() { return HashCode.Combine("farmitem", Kind); }
}

[Serializable]
public class Seed : Item
{
    public static readonly FarmItemKind[] Allowed = new FarmItemKind[] {
        FarmItemKind.Sugar,
        FarmItemKind.Ice,
        FarmItemKind.Cocoa,
        FarmItemKind.Milk
    };

    public Seed(FarmItemKind kind)
    {
        if (!Allowed.Contains(kind)) throw new Exception($"Invalid seed type: {kind}");
        Kind = kind;
    }

    public FarmItemKind Kind;


    public override int MaxStackCount => 1;
    public override Sprite Sprite => Resources.Load<Sprite>("Items/Parsnip_Seeds");

    public override bool Equals(object obj) { return obj is Seed s && s.Kind == Kind; }
    public override int GetHashCode() { return HashCode.Combine("seed", Kind); }
}


static class FarmItemMethods
{
    public static int GetTier(this FarmItemKind i)
    {
        switch (i)
        {
            case FarmItemKind.Sugar:
            case FarmItemKind.Ice:
            case FarmItemKind.Cocoa:
            case FarmItemKind.Milk:
                return 0;

            case FarmItemKind.Candy:
            case FarmItemKind.Chantilly:
            case FarmItemKind.Popsicle:
            case FarmItemKind.DarkChocolate:
                return 1;


            case FarmItemKind.Milkshake:
            case FarmItemKind.Sorbet:
            case FarmItemKind.Chocolate:
            case FarmItemKind.SweetenedCondensedMilk:
                return 2;

            case FarmItemKind.Magnum:
            case FarmItemKind.IceCream:
            case FarmItemKind.Smarties:
                return 3;
        }

        return -1;
    }
}

public class ItemManager : MonoBehaviour
{
    public List<FarmItemKind> AvailableItems { get; private set; }
    public Dictionary<FarmItemKind, Sprite> Sprites { get; private set; }

    public static ItemManager Instance { get; private set; }

    public int ItemPrice(FarmItemKind item)
    {
        return item.GetTier();
    }

    void Start()
    {
        Instance = this;

        Sprites = new Dictionary<FarmItemKind, Sprite>();

        foreach (var i in Enum.GetValues(typeof(FarmItemKind)).Cast<FarmItemKind>())
        {
            var sprite = Resources.Load<Sprite>("Items/" + i.ToString().ToLower());
            if (sprite == null)
            {
                print($"Unable to get sprite for {i}");
            }

            Sprites.Add(i, sprite);
        }
    }
}

[Serializable]
public struct ItemStack
{
    [SerializeReference, SubclassSelector] public Item item;
    public int count;

    public ItemStack(Item i, int c)
    {
        item = i;
        count = c;
    }

    public override bool Equals(object obj) { return obj is ItemStack s && s.item.Equals(item) & s.count == count; }
    public override int GetHashCode() { return HashCode.Combine(item, count); }
}