using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using static FarmItemKind;

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
    public override Sprite Sprite => Resources.Load<Sprite>("Items/Farm/" + Kind.ToString().ToLower());
    public override bool Equals(object obj) { return obj is FarmItem s && s.Kind == Kind; }
    public override int GetHashCode() { return HashCode.Combine("farmitem", Kind); }
}

[Serializable]
public class Seed : Item
{
    public static readonly FarmItemKind[] Allowed = new FarmItemKind[] {
        Sugar,
        Ice,
        Cocoa,
        Milk
    };

    public Seed(FarmItemKind kind)
    {
        if (!Allowed.Contains(kind)) throw new Exception($"Invalid seed type: {kind}");
        Kind = kind;
    }

    public FarmItemKind Kind;


    public override int MaxStackCount => 1;
    public override Sprite Sprite => Resources.Load<Sprite>("Items/Seeds/" + Kind.ToString() + "Seeds");

    public override bool Equals(object obj) { return obj is Seed s && s.Kind == Kind; }
    public override int GetHashCode() { return HashCode.Combine("seed", Kind); }
}


static class FarmItemMethods
{
    public static int GetTier(this FarmItemKind i)
    {
        switch (i)
        {
            case Sugar:
            case Ice:
            case Cocoa:
            case Milk:
                return 0;

            case Candy:
            case Chantilly:
            case Popsicle:
            case DarkChocolate:
                return 1;


            case Milkshake:
            case Sorbet:
            case Chocolate:
            case SweetenedCondensedMilk:
                return 2;

            case Magnum:
            case IceCream:
            case Smarties:
                return 3;
        }

        return -1;
    }
}

public class ItemManager : MonoBehaviour
{
    private static FarmItemKind[][] ItemsByTier =
    {
      new FarmItemKind[]{Sugar, Ice, Milk, Cocoa},
      new FarmItemKind[]{Candy, Chantilly, Popsicle, DarkChocolate},
      new FarmItemKind[]{Milkshake, Sorbet, Chocolate, SweetenedCondensedMilk},
      new FarmItemKind[]{Magnum, IceCream, Smarties},
    };

    public List<FarmItemKind> AvailableItems { get; private set; } = new();
    public List<Recipe> AvailableRecipes { get; private set; } = new();

    public static ItemManager Instance { get; private set; }

    public int ItemPrice(FarmItemKind item)
    {
        return item.GetTier();
    }

    void Start()
    {
        Instance = this;

        AvailableItems = new();
        AvailableRecipes = new();

        var initialItem = ItemsByTier[0][UnityEngine.Random.Range(0, ItemsByTier[0].Length)];
        AvailableItems.Add(initialItem);
    }

    public void UpgradeTier()
    {
        FarmItemKind? toadd = null;
        while (toadd == null)
        {
            var item = ItemsByTier[0][UnityEngine.Random.Range(0, ItemsByTier[0].Length)];
            if (!AvailableItems.Contains(item)) toadd = item;
        }

        AvailableItems.Add(toadd ?? throw new Exception());

        for (int i = 1; i <= StateManager.Instance.Tier; ++i)
        {
            AddRandomRecipe(i);
        }
    }

    private void AddRandomRecipe(int tier)
    {
        Recipe toadd = null;
        while (toadd == null)
        {
            var available = Recipe.ByTier[tier];
            var idx = UnityEngine.Random.Range(0, available.Length);
            print(available);
            print(idx);
            var r = available[idx];

            if (!AvailableRecipes.Contains(r)
                && AvailableItems.Contains(r.Items[0])
                && AvailableItems.Contains(r.Items[1]))
                toadd = r;
        }

        AvailableItems.Add(toadd.Output);
        AvailableRecipes.Add(toadd);

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

    public override readonly bool Equals(object obj) { return obj is ItemStack s && s.item.Equals(item) & s.count == count; }
    public override readonly int GetHashCode() { return HashCode.Combine(item, count); }
}

[Serializable]
public class Recipe
{
    public Recipe(FarmItemKind a, FarmItemKind b, FarmItemKind output)
    {
        Items = new FarmItemKind[] { a, b };
        Output = output;
    }

    public readonly FarmItemKind[] Items;
    public readonly FarmItemKind Output;


    public static Recipe[][] ByTier =
    {
      new Recipe[]{},
      new Recipe[]{ new(Sugar, Sugar, Candy), new(Cocoa, Cocoa, DarkChocolate), new(Milk, Milk, Chantilly), new(Ice, Ice, Popsicle) },
      new Recipe[]{ new(Sugar, Ice, Sorbet), new(Sugar, Milk, SweetenedCondensedMilk), new(Cocoa, Milk, Chocolate), new(Milk, Ice, Milkshake) },
      new Recipe[]{ new(Sorbet, Milk, IceCream), new(Sugar, Chocolate, Smarties) },
    };
}
