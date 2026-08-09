using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public TextMeshProUGUI countText;
    public TextMeshProUGUI timerText;
    public Image itemImage;
    public Quest quest;

    void Update()
    {
        if (quest != null)
        {
            timerText.text = quest.Timer.ToString();
        }
    }
}
