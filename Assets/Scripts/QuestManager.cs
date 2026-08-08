using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    [SerializeField] private int[] minQuestTimer;
    [SerializeField] private int[] maxQuestTimer;
    [SerializeField] private int maxFailureCount;

    public List<Quest> PendingQuests;

    private int questCount = 0;
    private int tickBeforeNextQuest = 0;
    private int failureCount;

    void Start()
    {
        Instance = this;

        PendingQuests = Enumerable.Repeat<Quest>(null, maxQuests.Max()).ToList();

        var farm = GetComponent<StateManager>();
        tickBeforeNextQuest = Random.Range(minQuestDelay[farm.Tier], maxQuestDelay[farm.Tier]);
    }

    public void NewQuest()
    {
        var farm = GetComponent<StateManager>();
        tickBeforeNextQuest = Random.Range(minQuestDelay[farm.Tier], maxQuestDelay[farm.Tier]);

        // Ignore quest generation if all slots are used
        if (questCount >= maxQuests[farm.Tier]) { return; }

        // Chooses a random item in the available ones.
        var randIdx = Random.Range(0, ItemManager.Instance.AvailableItems.Count);
        var item = ItemManager.Instance.AvailableItems[randIdx];

        // Computes the amount of items for the quest.
        var weight = Random.Range(minWeights[farm.Tier], maxWeights[farm.Tier]);
        var timer = Random.Range(minQuestTimer[farm.Tier], maxQuestDelay[farm.Tier]);
        var count = weight / (item.GetTier() + 1);
        var quest = new Quest(new ItemStack(new FarmItem(item), count), timer);

        var idx = PendingQuests.FindIndex(e => e == null);
        if (idx == -1) { PendingQuests.Add(quest); }
        else { PendingQuests[idx] = quest; }

        questCount++;
    }

    private void CompleteQuest(int slot)
    {
        var quest = PendingQuests[slot] ?? throw new System.Exception();
        StateManager.Instance.Wheat += quest.Stack.count * GetComponent<ItemManager>().ItemPrice((quest.Stack.item as FarmItem).Kind);
        PendingQuests[slot] = null;
    }


    public bool DepositPackage(QuestPackage package, int slot)
    {
        if (PendingQuests[slot] != null && PendingQuests[slot].Stack.Equals(package.Stack))
        {
            CompleteQuest(slot);
            return true;
        }
        return false;
    }

    public override void Tick()
    {
        TickQuestTimer();

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

    private void TickQuestTimer()
    {
        for (var i = 0; i < PendingQuests.Count; ++i)
        {
            var q = PendingQuests[i];
            if (q != null && q.Timer > 0)
            {
                q.Timer -= 1;
                if (q.Timer == 0)
                {
                    FailQuest(i);
                }
            }
        }
    }

    private void FailQuest(int slot)
    {
        print("fail");

        failureCount += 1;
        PendingQuests[slot] = null;
        questCount -= 1;

        if (failureCount == maxFailureCount)
        {
            print("Defeat!");
        }
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


[System.Serializable]
public class Quest
{

    public Quest(ItemStack stack, int timer)
    {
        Stack = stack;
        Timer = timer;
    }

    public ItemStack Stack;
    public int Timer;
}
