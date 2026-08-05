using UnityEngine;

public class ItemStackDisplay : MonoBehaviour
{
    public ItemStack Stack = new(FarmItem.Sugar, 3);

    [SerializeField] private SpriteRenderer[] cells;
    private ItemManager itemManager;


    void Start()
    {
        itemManager = GameObject.FindWithTag("GameManager").GetComponent<ItemManager>();
    }

    void Update()
    {
        var sprite = itemManager.Sprites[Stack.item];

        for (int i = 0; i < cells.Length; ++i)
        {
            cells[i].enabled = i < Stack.count;
            cells[i].sprite = sprite;
        }
    }
}
