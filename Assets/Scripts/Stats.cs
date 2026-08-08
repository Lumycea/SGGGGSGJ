using UnityEngine;

public class Stats : MonoBehaviour
{
    public bool Victory = false;
    public int WheatGained = 0;
    public int ResourcesSold = 0;
    public int QuestsCompleted = 0;
    public int PlayersLost = 0;

    public static Stats Instance;

    void Start()
    {
        Instance = this;
    }
}
