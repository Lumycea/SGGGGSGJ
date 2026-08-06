using UnityEngine;

public class QuestPoint : MonoBehaviour
{
    [SerializeField] private int slotIndex;

    public bool HasQuest()
    {
        return QuestManager.Instance.PendingQuests[slotIndex] != null;
    }

    public Item GetTicket()
    {
        return new QuestTicket(QuestManager.Instance.PendingQuests[slotIndex] ?? throw new System.Exception());
    }

    public bool DepositPackage(QuestPackage package)
    {
        return QuestManager.Instance.DepositPackage(package, slotIndex);
    }
}
