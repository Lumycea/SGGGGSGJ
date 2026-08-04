using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class PlayerCounterText : MonoBehaviour
{
    void Update()
    {
        int playerCount = PlayerInput.all.Count;
        GetComponent<TMPro.TextMeshProUGUI>().text = "Players: " + playerCount;
    }
}
