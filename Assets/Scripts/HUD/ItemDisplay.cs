using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplay : MonoBehaviour
{
    private Sprite _sprite = null;
    private int _count = 0;
    private int _delta = 0;
    private bool _showDelta = false;

    public Sprite Sprite { get => _sprite; set { _sprite = value; ItemImage.sprite = Sprite; } }
    public int Count { get => _count; set { _count = value; updateText(); } }
    public int Delta { get => _delta; set { _delta = value; updateText(); } }
    public bool ShowDelta { get => _showDelta; set { _showDelta = value; updateText(); } }

    public Image ItemImage;
    public TMP_Text StatusText;

    // Update is called once per frame
    void updateText()
    {
        ItemImage.sprite = Sprite;
        StatusText.text = !ShowDelta ? Count.ToString() : Delta > 0 ? $"{Count} (+{Delta})" : $"{Count} ({Delta})";
    }
}
