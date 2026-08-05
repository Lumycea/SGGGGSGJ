using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public enum FarmItem
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


static class FarmItemMethods
{
    public static int GetTier(this FarmItem i)
    {
        switch (i)
        {
            case FarmItem.Sugar:
            case FarmItem.Ice:
            case FarmItem.Cocoa:
            case FarmItem.Milk:
                return 0;

            case FarmItem.Candy:
            case FarmItem.Chantilly:
            case FarmItem.Popsicle:
            case FarmItem.DarkChocolate:
                return 1;


            case FarmItem.Milkshake:
            case FarmItem.Sorbet:
            case FarmItem.Chocolate:
            case FarmItem.SweetenedCondensedMilk:
                return 2;

            case FarmItem.Magnum:
            case FarmItem.IceCream:
            case FarmItem.Smarties:
                return 3;
        }

        return -1;
    }
}

public class ItemManager : MonoBehaviour
{
    public List<FarmItem> AvailableItems { get; private set; }
    public Dictionary<FarmItem, Sprite> Sprites { get; private set; }

    public int ItemPrice(FarmItem item)
    {
        return item.GetTier();
    }

    void Start()
    {
        Sprites = new Dictionary<FarmItem, Sprite>();

        foreach (var i in Enum.GetValues(typeof(FarmItem)).Cast<FarmItem>())
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
    public FarmItem item;
    public int count;

    public ItemStack(FarmItem i, int c)
    {
        item = i;
        count = c;
    }
}