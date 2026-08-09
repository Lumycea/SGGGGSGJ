using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject itemDisplayPrefab;
    [SerializeField] private Transform inventoryPanel;
    [SerializeField] private TMP_Text wheatCount;

    private Dictionary<Player, GameObject> playerHeads = new();
    [SerializeField] private Transform playerHeadsParent;
    [SerializeField] private GameObject playerHeadPrefab;

    private Dictionary<Quest, GameObject> questPanels = new();
    [SerializeField] private Transform questPanelsParent;
    [SerializeField] private GameObject questPanelPrefab;

    private readonly Dictionary<FarmItemKind, ItemDisplay> displays = new();
    private readonly uint[] usedSlots = { 0, 0, 0, 0 };
    private ItemManager itemManager;

    public Dictionary<FarmItemKind, int> Inventory = new();
    public Dictionary<FarmItemKind, int> Delta = new();
    public bool ShowDelta = false;

    public static HUD Instance { get; private set; }

    void Start()
    {
        Instance = this;

        itemManager = GameObject.FindWithTag("GameManager").GetComponent<ItemManager>();
    }

    void Update()
    {
        foreach (var e in Farm.Instance.Inventory)
        {
            if (!displays.ContainsKey(e.Key))
            {
                var display = Instantiate(itemDisplayPrefab, inventoryPanel).GetComponent<ItemDisplay>();
                display.Sprite = new FarmItem(e.Key).Sprite;
                displays.Add(e.Key, display);
            }

            displays[e.Key].Count = e.Value;
            displays[e.Key].Delta = Delta.ContainsKey(e.Key) ? Delta[e.Key] : 0;
            displays[e.Key].ShowDelta = ShowDelta;
        }

        wheatCount.text = StateManager.Instance.Wheat.ToString();

        foreach (var player in PlayerManager.Instance.players.Values)
        {
            if (player.isInJail)
            {
                if (!playerHeads.ContainsKey(player))
                {
                    var head = Instantiate(playerHeadPrefab, playerHeadsParent);
                    head.GetComponent<Image>().color = player.playerColor;
                    playerHeads.Add(player, head);
                }
            }
            else
            {
                if (playerHeads.ContainsKey(player))
                {
                    Destroy(playerHeads[player]);
                    playerHeads.Remove(player);
                }
            }
        }

        for (int i = 0; i < QuestManager.Instance.MaxQuestCount; ++i)
        {
            if (!QuestManager.Instance.HasQuest(i)) continue;
            var quest = QuestManager.Instance.Quest(i);
            if (!questPanels.ContainsKey(quest))
            {
                var questUI = Instantiate(questPanelPrefab, questPanelsParent).GetComponent<QuestUI>();
                questUI.itemImage.sprite = quest.Stack.item.Sprite;
                questUI.countText.text = quest.Stack.count.ToString();
                questUI.quest = quest;
                quest.QuestPanel = questUI.gameObject;

                questPanels.Add(quest, questUI.gameObject);
            }
        }
    }
}
