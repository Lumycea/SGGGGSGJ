using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] private Sprite _sprite = null;
    [SerializeField] private int _count = 0;
    [SerializeField] private int _delta = 0;
    [SerializeField] private bool _showDelta = false;

    public Sprite Sprite { get => _sprite; set { _sprite = value; ItemImage.sprite = Sprite; } }
    public int Count { get => _count; set { _count = value; UpdateText(); } }
    public int Delta { get => _delta; set { _delta = value; UpdateText(); } }
    public bool ShowDelta { get => _showDelta; set { _showDelta = value; UpdateText(); } }

    public Image ItemImage;
    public TMP_Text StatusText;

    // Update is called once per frame
    void UpdateText()
    {
        ItemImage.sprite = Sprite;
        StatusText.text = !ShowDelta ? Count.ToString() : Delta > 0 ? $"{Count} (+{Delta})" : $"{Count} ({Delta})";
    }
}
