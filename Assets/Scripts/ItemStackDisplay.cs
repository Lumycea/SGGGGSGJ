using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemStackDisplay : MonoBehaviour
{
    public ItemStack Stack = new(FarmItem.Sugar, 1);
    public bool ForceBackground = false;

    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private SpriteRenderer itemRenderer;
    [SerializeField] private TMP_Text countText;

    private ItemManager itemManager;


    void Start()
    {
        itemManager = GameObject.FindWithTag("GameManager").GetComponent<ItemManager>();
    }

    void Update()
    {
        itemRenderer.sprite = itemManager.Sprites[Stack.item];

        countText.enabled = Stack.count > 1;
        countText.text = Stack.count.ToString();

        backgroundRenderer.enabled = Stack.count > 1 || ForceBackground;
    }
}
