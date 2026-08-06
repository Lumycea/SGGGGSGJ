using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private Crop[] crops;

    public bool CanPlant(FarmItemKind item)
    {
        foreach (var c in crops)
        {
            if (c.Item != null && c.Item != item)
            {
                return false;
            }
        }

        return true;
    }
}
