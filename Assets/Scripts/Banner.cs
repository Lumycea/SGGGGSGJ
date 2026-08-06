using UnityEngine;

public class Banner : Item
{
    public override int MaxStackCount => 1;

    public override Sprite Sprite => Resources.Load<Sprite>("Items/MilkShake");
}
