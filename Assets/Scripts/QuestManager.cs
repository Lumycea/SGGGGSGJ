using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ItemManager))]
[RequireComponent(typeof(StateManager))]
[RequireComponent(typeof(Farm))]
public class QuestManager : SimulationEntity
{
    public static QuestManager Instance;

    [SerializeField] private int[] maxQuests;
    [SerializeField] private int[] minWeights;
    [SerializeField] private int[] maxWeights;
    [SerializeField] private int[] minQuestDelay;
    [SerializeField] private int[] maxQuestDelay;

    public List<ItemStack?> PendingQuests { get; private set; }

    private int questCount = 0;
    private int tickBeforeNextQuest = 0;

    void Start()
    {
        Instance = this;


        PendingQuests = Enumerable.Repeat<ItemStack?>(null, maxQuests.Max()).ToList();

        var farm = GetComponent<StateManager>();
        tickBeforeNextQuest = Random.Range(minQuestDelay[farm.Tier], maxQuestDelay[farm.Tier]);
    }

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
        var count = weight / (item.GetTier() + 1);


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


    public bool DepositPackage(QuestPackage package, int slot)
    {
        if (PendingQuests[slot] is ItemStack stack && stack.Equals(package.Stack))
        {
            CompleteQuest(slot);
            return true;
        }
        return false;
    }

    public override void Tick()
    {
        if (!StateManager.Instance.canGenerateQuest)
        {
            return;
        }

        if (StateManager.Instance.generateQuestNow)
        {
            StateManager.Instance.generateQuestNow = false;
            NewQuest();
        }

        tickBeforeNextQuest--;
        if (tickBeforeNextQuest == 0) { NewQuest(); }
    }
}

[System.Serializable]
public class QuestTicket : Item
{
    public QuestTicket(ItemStack stack) { Stack = stack; }

    public readonly ItemStack Stack;

    public override int MaxStackCount => 1;
    public override Sprite Sprite => Resources.Load<Sprite>("Items/checklist");
}

[System.Serializable]
public class QuestPackage : Item
{
    public QuestPackage(ItemStack stack) { Stack = stack; }

    public readonly ItemStack Stack;

    public override int MaxStackCount => 1;
    public override Sprite Sprite => Resources.Load<Sprite>("Items/package");
}

