using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemManager))]
[RequireComponent(typeof(StateManager))]
public class QuestManager : SimulationEntity
{
    [SerializeField] private int[] maxQuests;
    [SerializeField] private int[] minWeights;
    [SerializeField] private int[] maxWeights;
    [SerializeField] private int[] minQuestDelay;
    [SerializeField] private int[] maxQuestDelay;

    public List<ItemStack?> PendingQuests { get; private set; } = new();

    private int questCount = 0;
    private int tickBeforeNextQuest = 0;

    public void NewQuest()
    {
        var itemManager = GetComponent<ItemManager>();
        var farm = GetComponent<StateManager>();

        // Ignore quest generation if all slots are used
        if (questCount >= maxQuests[farm.Tier]) { return; }

        // Chooses a random item in the available ones.
        var randIdx = Random.Range(0, itemManager.AvailableItems.Count);
        var item = itemManager.AvailableItems[randIdx];

        // Computes the amount of items for the quest.
        var weight = Random.Range(minWeights[farm.Tier], maxWeights[farm.Tier]);
        var count = weight / item.GetTier();


        var idx = PendingQuests.FindIndex(e => e == null);
        if (idx == -1) { PendingQuests.Add(new ItemStack(new FarmItem(item), count)); }
        else { PendingQuests[idx] = new ItemStack(new FarmItem(item), count); }

        questCount++;

        tickBeforeNextQuest = Random.Range(minQuestDelay[farm.Tier], maxQuestDelay[farm.Tier]);
    }

    private void CompleteQuest(int slot)
    {
        var quest = PendingQuests[slot] ?? throw new System.Exception();
        GetComponent<Farm>().Wheat += quest.count * GetComponent<ItemManager>().ItemPrice((quest.item as FarmItem).Kind);
        PendingQuests[slot] = null;
    }

    /// <summary>
    /// Attempts to deposit an item stack at a quest slot. If the quest cannot be completed with this stack, returns it and does nothing.
    /// </summary>
    /// <param name="stack"></param>
    /// <param name="slot"></param>
    /// <returns>The remaining item stack, or null if everything is used.</returns>
    public ItemStack? DepositStack(ItemStack stack, int slot)
    {
        var questN = PendingQuests[slot];

        if (questN is ItemStack quest)
        {
            // Stack item doesn't match the quest, or there are not enought items.
            if (quest.item != stack.item || quest.count > stack.count) { return stack; }

            CompleteQuest(slot);

            if (quest.count == stack.count) { return null; }
            else { return new ItemStack(stack.item, stack.count - quest.count); }
        }
        return stack;
    }


    public override void Tick()
    {
        tickBeforeNextQuest--;
        if (tickBeforeNextQuest == 0) { NewQuest(); }
    }
}
